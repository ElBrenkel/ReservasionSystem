using ReservationSystemBusinessLogic.Log;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Context
{
    public class ReservationLogContext : DbContext
    {
        public ReservationLogContext() : base(Constants.ConnectionString) { }

        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
