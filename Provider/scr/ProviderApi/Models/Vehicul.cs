using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProviderApi.Models
{
    public class Vehicul
    {
        [JsonProperty("vehiculId")]
        public int VehiculId { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
