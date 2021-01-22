using ReservationSystemBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace ReservationSystemBusinessLogic.Objects
{
    public class WorkingHours
    {
        public long Id{ get; set; }

        public long? RoomId{ get; set; }

        [ForeignKey("RoomId")]
        public Room Room{ get; set; }

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