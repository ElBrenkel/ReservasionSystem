using ReservationSystemBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Objects.Api
{
    public class UserResponse : ChangeUserDataPayload
    {
        public string Username { get; set; }
    }
}
