using ReservationSystemBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace ReservationSystemBusinessLogic.Objects
{
    [Table("ReservationRequests")]
    public class ReservationRequest
    {
        public long Id { get; set; }

        public long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        
        public long? RoomId { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        public DateTime RentStart { get; set; }

        public DateTime RentEnd { get; set; }

        public decimal FinalPrice { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public ReservationStatus Status { get; set; }
    }
}