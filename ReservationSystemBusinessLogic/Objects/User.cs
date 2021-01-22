using ReservationSystemBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ReservationSystemBusinessLogic.Objects
{
    [Table("Users")]
    public class User
    {
        public long Id { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(500)]
        public string PasswordHash { get; set; }

        public UserRole Role { get; set; }

        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string Country { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        public int? BuildingNumber { get; set; }

        public Guid? Token { get; set; }

        public DateTime? TokenExpiryDate { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}