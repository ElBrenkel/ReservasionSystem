using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystemBusinessLogic.Common
{
    public static class PriceHelper
    {
        public static decimal CalculatePrice(decimal priceForHour, DateTime rentStart, DateTime rentEnd)
        {
            decimal priceForMinute = priceForHour / 60;
            decimal timeInMinutes = rentEnd.ToMinutes() - rentStart.ToMinutes();
            return priceForMinute * timeInMinutes;
        }
    }
}
