using ConsumerApp;
using ConsumerApp.Models;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Consumer.Tests.Pact
{
    public class SimpleConsumerApiTest : IClassFixture<ConsumerPactClassFixture>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public SimpleConsumerApiTest(ConsumerPactClassFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;
        }

        [Fact]
        public void Return_Valid_Vehicul_Given_A_Specific_Id()
        {
            #region Arrange
            int vehiculId = 2;
            _mockProviderService.Given("FullVehiculState")
                .UponReceiving("A valid GET request for specific vehicul with valid id parameter")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/api/Vehiculs/{vehiculId}"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new Vehicul { VehiculId = 2, Label = "Powered Car" }
                });

            #endregion

            #region Act
            var result = SimpleConsumerApiClient.GetAllVehiculById(2, _mockProviderServiceBaseUri).GetAwaiter().GetResult();
            #endregion

            #region Assert
            Assert.Equal(2, result.VehiculId);
            Assert.Equal("Powered Car", result.Label);
            #endregion
        }

        [Fact]
        public void Return_Valid_Vehicul_List()
        {
            #region Arrange
            _mockProviderService.Given("FullVehiculState")
                .UponReceiving("A valid GET request for all vehiculs with no parameter")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/api/Vehiculs"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new Vehicul[]
                    {
                        new Vehicul() { VehiculId = 1, Label = "Simple Car" },
                        new Vehicul() { VehiculId = 2, Label = "Powered Car" },
                        new Vehicul() { VehiculId = 3, Label = "Three wheel Car" }
                    }
                });

            #endregion

            #region Act
            var result = SimpleConsumerApiClient.GetAllVehiculs(_mockProviderServiceBaseUri).GetAwaiter().GetResult();
            #endregion

            #region Assert
            Assert.Equal(3, result.Length);
            Assert.Equal(1, result[0].VehiculId);
            Assert.Equal("Simple Car", result[0].Label);
            Assert.Equal(2, result[1].VehiculId);
            Assert.Equal("Powered Car", result[1].Label);
            Assert.Equal(3, result[2].VehiculId);
            Assert.Equal("Three wheel Car", result[2].Label);
            #endregion
        }
    }
}
