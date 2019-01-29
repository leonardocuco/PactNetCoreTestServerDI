using ConsumerApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerApp
{
    public static class SimpleConsumerApiClient
    {
        static public async Task<Vehicul[]> GetAllVehiculs(string baseUri)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(baseUri) })
            {
                try
                {
                    var response = await client.GetAsync($"/api/Vehiculs").GetAwaiter().GetResult()
                        .Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<Vehicul[]>(response);
                }
                catch (System.Exception ex)
                {
                    throw new Exception("There was a problem connecting to Provider API.", ex);
                }
            }
        }

        static public async Task<Vehicul> GetAllVehiculById(int id, string baseUri)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(baseUri) })
            {
                try
                {
                    var response = await client.GetAsync($"/api/Vehiculs/{id}").GetAwaiter().GetResult()
                        .Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<Vehicul>(response);
                }
                catch (System.Exception ex)
                {
                    throw new Exception("There was a problem connecting to Provider API.", ex);
                }
            }
        }
    }
}
