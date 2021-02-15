using Newtonsoft.Json;
using ReservationSystemBusinessLogic.ApiClients.Objects;
using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.ApiClients
{
    public class MapQuestClient : IGeolocationClient
    {
        private readonly string BaseUrl = "http://www.mapquestapi.com/geocoding/v1";
        private readonly string ApiKey = "eGpfYFQmAVu6Om231QJ9bTqZxJCZudW8";

        public async Task<LatLon> ForwardGeolocation(string county, string city, string street, int? buildingNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                string requestUrl = $"{BaseUrl}/address?key={ApiKey}";
                MapQuestRequestBody body = new MapQuestRequestBody(county, city, street, buildingNumber);
                HttpContent content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                HttpResponseMessage message = await client.PostAsync(requestUrl, content);
                string response = await message.Content.ReadAsStringAsync();
                dynamic responseAsDynamic = JsonConvert.DeserializeObject(response);
                return new LatLon()
                {
                    Lat = responseAsDynamic.results[0].locations[0].latLng.lat,
                    Lon = responseAsDynamic.results[0].locations[0].latLng.lng
                };
            }
        }
    }
}
