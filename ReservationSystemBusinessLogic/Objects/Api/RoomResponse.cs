using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Objects.Api
{
    public class RoomResponse : CreateRoomPayload
    {
        public long Id { get; set; }
    }
}
