using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ReservationSystemBusinessLogic.Context;

namespace ReservationSystemBusinessLogic.Log
{
    public static class Logger
    {
        static Logger()
        {
            using (ReservationLogContext logContext = new ReservationLogContext())
            {
                logContext.LogEntries.Create();
            }
        }

        private static void Log(string level, string message)
        {
            (string type, string name) callerData = GetCallerData();
            using (ReservationLogContext logContext = new ReservationLogContext())
            {
                logContext.LogEntries.Add(new LogEntry()
                {
                    Level = level,
                    Time = DateTime.UtcNow,
                    Type = callerData.type,
                    Method = callerData.name,
                    Message = message
                });
                logContext.SaveChanges();
            }
        }

        public static void Debug(string message)
        {
            Log("DEBUG", message);
        }

        public static void Error(string message)
        {
            Log("ERROR", message);
        }

        private static (string type, string name) GetCallerData()
        {
            StackFrame frame = new StackFrame(3);
            var method = frame.GetMethod();
            var type = method.DeclaringType.Name;
            var name = method.Name;
            return (type, name);
        }

    }
}
