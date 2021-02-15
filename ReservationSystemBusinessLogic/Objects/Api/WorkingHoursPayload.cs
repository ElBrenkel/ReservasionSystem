using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ReservationSystemBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Objects.Api
{
    public class WorkingHoursPayload
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Days Day { get; set; }

        /// <summary>
        /// Time in minutes from 00:00 untill desired time (08:00 is 480).
        /// </summary>
        public int TimeStart { get; set; }

        /// <summary>
        /// Time in minutes from 00:00 untill desired time (08:00 is 480).
        /// </summary>
        public int TimeEnd { get; set; }

        public decimal PriceForHour { get; set; }
    }
}
