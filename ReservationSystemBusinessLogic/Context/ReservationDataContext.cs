using ReservationSystemBusinessLogic.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace ReservationSystemBusinessLogic.Context
{
    public class ReservationDataContext : DbContext
    {
        public ReservationDataContext() : base(Constants.ConnectionString) { }

        public DbSet<WorkingHours> WorkingHours { get; set; }
        public DbSet<ReservationRequest> ReservationRequests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }

        public void CreateTables()
        {
            Users.Create();
            Rooms.Create();
            WorkingHours.Create();
            WorkingHours.Create();
        }
    }
}
