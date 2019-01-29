using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProviderApi.Repositories;
using ProviderApi.Tests.Pacts.TestRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using ProviderApi.Tests.Pacts.HttpHelpers;

namespace ProviderApi.Tests.Pacts.Middlewares
{
    public class InMemoryProviderStateMiddleware : IDisposable
    {
        private const string ConsumerName = "Consumer";
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, Action> _providerStates;
        private HttpClient _testClient;
        private TestServer _testServer;

        public InMemoryProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
            _providerStates = new Dictionary<string, Action>
            {
                {
                    "FullVehiculState",
                    SetFullVehiculState
                }
            };
        }

        #region Provider States
        private void SetFullVehiculState()
        {
            // On définit sur le handler de ce Provider State le testClient pointant vers un WebHost In memory 
            // faisant faisant tourner notre ProviderApi avec le repository correpondant aux données attendue pour
            // notre State, ici le FullVehiculRepository.
            _testServer = new TestServer(Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
              .ConfigureServices(serviceCollection =>
                serviceCollection.AddScoped<IVehiculRepository, FullVehiculRepository>())
              .UseStartup<ProviderApi.Startup>());
            _testClient = _testServer.CreateClient();
        } 
        #endregion

        #region Rooting
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == "/provider-states")
            {
                this.HandleProviderStatesRequest(context);
                await context.Response.WriteAsync(String.Empty);
            }
            else
            {
                // Maintenant, au lieu de propager l'appel vers sa destination avec await this._next(context),
                // on reroot celui-ci vers notre TestServer, la réponse sera placée dans le context.
                await RootHtppRequestThroughTestServer(context);
            }
        }

        private void HandleProviderStatesRequest(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (context.Request.Method.ToUpper() == HttpMethod.Post.ToString().ToUpper() &&
                context.Request.Body != null)
            {
                string jsonRequestBody = String.Empty;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = reader.ReadToEnd();
                }

                var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                //A null or empty provider state key must be handled
                if (providerState != null && !String.IsNullOrEmpty(providerState.State) &&
                    providerState.Consumer == ConsumerName)
                {
                    _providerStates[providerState.State].Invoke();
                }
            }
        }

        private async Task RootHtppRequestThroughTestServer(HttpContext context)
        {
            var response = await _testClient.SendAsync(context.Request.ToHttpRequestMessage());
            context.Response.StatusCode = (int)response.StatusCode;
            context.Response.ContentType = response.Content.Headers.ContentType.ToString();
            await context.Response.WriteAsync(await response.Content.ReadAsStringAsync());
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _testServer != null)
                {
                    _testServer.Host.StopAsync().GetAwaiter().GetResult();
                    _testServer.Dispose();

                    if(_testClient != null)
                    {
                        _testClient.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
