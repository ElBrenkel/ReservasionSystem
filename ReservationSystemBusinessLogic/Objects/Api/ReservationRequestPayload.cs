using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Objects.Api
{
    public class ReservationRequestPayload
    {
        public DateTime RentStart { get; set; }

        public DateTime RentEnd { get; set; }

        public string Description { get; set; }
    }
}
