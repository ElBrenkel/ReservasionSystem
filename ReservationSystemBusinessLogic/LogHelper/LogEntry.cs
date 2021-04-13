using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Log
{
    [Table("ReservationSystemLog")]
    public class LogEntry
    {
        public long Id { get; set; }

        public DateTime Time { get; set; }

        [StringLength(10)]
        public string Level { get; set; }

        [StringLength(100)]
        public string Type { get; set; }

        [StringLength(100)]
        public string Method { get; set; }

        [StringLength(1000)]
        public string Message { get; set; }
    }
}
