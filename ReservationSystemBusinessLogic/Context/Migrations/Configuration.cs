using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Context.Migrations
{
    public class Configuration : DbMigrationsConfiguration<ReservationDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ReservationSystemBusinessLogic.Context.ReservationDataContext";
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
