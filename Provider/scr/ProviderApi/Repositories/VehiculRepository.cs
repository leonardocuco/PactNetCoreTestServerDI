using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderApi.Models;

namespace ProviderApi.Repositories
{
    public class VehiculRepository : IVehiculRepository
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
