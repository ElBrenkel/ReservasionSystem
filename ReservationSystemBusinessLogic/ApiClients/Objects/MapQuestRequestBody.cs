using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.ApiClients.Objects
{
    public class MapQuestRequestBody
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("options")]
        public Dictionary<string, bool> Options { get; set; }

        public MapQuestRequestBody() { }

        public MapQuestRequestBody(string county, string city, string street, int? buildingNumber)
        {
            Location = $"{street} {buildingNumber}, {city}, {county}";
            Options = new Dictionary<string, bool>() { { "thumbMaps", false } };
        }
    }
}
