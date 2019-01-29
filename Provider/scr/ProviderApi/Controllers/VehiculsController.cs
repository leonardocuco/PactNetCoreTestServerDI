using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProviderApi.Models;
using ProviderApi.Repositories;

namespace ProviderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculsController : ControllerBase
    {
        IVehiculRepository _vehiculRepo;

        public VehiculsController(IVehiculRepository vehiculRepo)
        {
            _vehiculRepo = vehiculRepo;
        }

        // GET: api/Vehiculs
        [HttpGet]
        public IEnumerable<Vehicul> Get()
        {
            return _vehiculRepo.GetAllVehiculs();
        }

        // GET: api/Vehiculs/5
        [HttpGet("{id}", Name = "Get")]
        public Vehicul Get(int id)
        {
            return _vehiculRepo.GetVehiculById(id);
        }

    }
}
