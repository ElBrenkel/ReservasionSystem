using ReservationSystemBusinessLogic.Context.Attributes;
using ReservationSystemBusinessLogic.Context.Migrations;
using ReservationSystemBusinessLogic.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace ReservationSystemBusinessLogic.Context
{
    public class ReservationDataContext : DbContext
    {
        public ReservationDataContext() : base(Constants.ConnectionString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ReservationDataContext, Configuration>());
        }

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

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new DecimalPrecisionAttributeConvention());
            base.OnModelCreating(modelBuilder);
        }
    }
}