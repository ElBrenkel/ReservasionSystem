using ReservationSystemBusinessLogic.Objects.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.ApiClients
{
    public interface IGeolocationClient
    {
        Task<LatLon> ForwardGeolocation(string county, string city, string street, int? buildingNumber);
    }
}
