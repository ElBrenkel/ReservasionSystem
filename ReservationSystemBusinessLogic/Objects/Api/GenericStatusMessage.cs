using System;
using System.Collections.Generic;
using System.Text;

namespace ReservationSystemBusinessLogic.Objects.Api
{
    public class GenericStatusMessage
    {
        public GenericStatusMessage(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }

        public string Message{ get; set; }
    }
}
