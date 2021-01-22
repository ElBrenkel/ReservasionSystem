using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationSystemApi.Objects
{
    public class UserLoginPayload
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
