using ProviderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProviderApi.Repositories
{
    public interface IVehiculRepository
    {
        Vehicul[] GetAllVehiculs();
        Vehicul GetVehiculById(int id);
    }
}
