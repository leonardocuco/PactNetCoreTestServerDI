using ProviderApi.Models;
using ProviderApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProviderApi.Tests.Pacts.TestRepositories
{
    class FullVehiculRepository : IVehiculRepository
    {
        public Vehicul[] GetAllVehiculs()
        {
            return new Vehicul[]
            {
                new Vehicul() { VehiculId = 1, Label = "Simple Car" },
                new Vehicul() { VehiculId = 2, Label = "Powered Car" },
                new Vehicul() { VehiculId = 3, Label = "Three wheel Car" }
            };
        }

        public Vehicul GetVehiculById(int id)
        {
            return GetAllVehiculs().FirstOrDefault(x => x.VehiculId == id);
        }
    }
}
