using ReservationSystemBusinessLogic.Context.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ReservationSystemBusinessLogic.Objects
{
    [Table("Rooms")]
    public class Room
    {
        public long Id { get; set; }

        public long? OwnerId { get; set; }

        [ForeignKey ("OwnerId")]
        public User Owner { get; set; }

        /// <summary>
        /// Calculated by the square metre.
        /// </summary>
        public int Size { get; set; }

        public int MaxNumberOfPeople { get; set; }

        [StringLength(20)]
        public string Country { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        public int? BuildingNumber { get; set; }

        [DecimalPrecision(10,6)]
        public decimal? Lat { get; set; }

        [DecimalPrecision(10, 6)]
        public decimal? Lon { get; set; }

        /// <summary>
        /// Used to indicate if the room is not closed.
        /// </summary>
        public bool IsActive { get; set; }

        public ICollection <WorkingHours> WorkingHours { get; set; }

        public ICollection <ReservationRequest> Reservations { get; set; }
    }
}