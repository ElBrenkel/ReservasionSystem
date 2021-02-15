using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Objects.Api
{
    public class GenericListResponse<T>
    {
        public GenericStatusMessage Status { get; set; }

        public List<T> Items { get; set; }

        public int TotalCount { get; set; }

        public GenericListResponse(List<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
            Status = new GenericStatusMessage(true);
        }

        public GenericListResponse(string failueMessage)
        {
            Status = new GenericStatusMessage(false, failueMessage);
            Items = null;
            TotalCount = 0;
        }
    }
}
