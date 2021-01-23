using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Objects.Api
{
    public class GenericObjectResponse<T>
    {
        public GenericStatusMessage Status { get; set; }
        public T Object { get; set; }

        public GenericObjectResponse(T obj)
        {
            Status = new GenericStatusMessage(true);
            Object = obj;
        }

        public GenericObjectResponse(string failueMessage)
        {
            Status = new GenericStatusMessage(false, failueMessage);
            Object = default(T);
        }
    }
}
