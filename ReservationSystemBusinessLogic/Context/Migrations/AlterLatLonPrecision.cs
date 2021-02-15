using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Context.Migrations
{
    class AlterLatLonPrecision : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rooms", "Lat", c => c.Decimal(nullable: true, precision: 10, scale: 6));
            AlterColumn("dbo.Rooms", "Lon", c => c.Decimal(nullable: true, precision: 10, scale: 6));
        }
    }
}
