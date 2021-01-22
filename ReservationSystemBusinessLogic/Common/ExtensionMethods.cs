using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ReservationSystemBusinessLogic.Common
{
    public static class ExtensionMethods
    {
        public static bool Matches(this string input, string pattern)
        {
            if (input == null)
            {
                return false;
            }

            Regex r = new Regex(pattern);
            return r.IsMatch(input);
        }

        public static bool CheckLength(this string input, int maxLength, int minLength = 0, bool required = true)
        {
            if  (input == null)
            {
                return !required;
            }

            return (input.Length >= minLength && input.Length <= maxLength);
        }


    }
}
