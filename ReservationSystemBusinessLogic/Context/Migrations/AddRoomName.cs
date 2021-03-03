using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Context.Migrations
{
    public class AddRoomName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "Name", c => c.String(false, 100, unicode: true, defaultValue: ""));
        }
    }
}
