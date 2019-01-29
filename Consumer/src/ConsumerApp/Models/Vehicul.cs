using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsumerApp.Models
{
    public class Vehicul
    {
        [JsonProperty("vehiculId")]
        public int VehiculId { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
