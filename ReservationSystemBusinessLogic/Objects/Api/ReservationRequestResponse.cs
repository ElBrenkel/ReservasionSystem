using ReservationSystemBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Objects.Api
{
    public class ReservationRequestResponse
    {

        public long? UserId { get; set; }

        public long? RoomId { get; set; }

        public DateTime RentStart { get; set; }

        public DateTime RentEnd { get; set; }

        public decimal? FinalPrice { get; set; }

        public string Description { get; set; }

        public ReservationStatus Status { get; set; }
    }
}
