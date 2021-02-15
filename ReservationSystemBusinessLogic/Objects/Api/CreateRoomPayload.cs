using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Objects.Api
{
    public class CreateRoomPayload
    {
        public int? Size { get; set; }

        public int? MaxNumberOfPeople { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int? BuildingNumber { get; set; }

        public decimal? Lat { get; set; }

        public decimal? Lon { get; set; }

        public bool IsActive { get; set; }

        public List<WorkingHoursPayload> WorkingHours { get; set; }
    }
}
