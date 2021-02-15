using ReservationSystemBusinessLogic.Enums;
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
            if (input == null)
            {
                return !required;
            }

            return (input.Length >= minLength && input.Length <= maxLength);
        }

        public static bool CheckIntFromZero(this int? input, bool isGreater, bool required = true)
        {
            if (input == null)
            {
                return !required;
            }

            return (isGreater ? input >= 0 : input <= 0);
        }

        public static bool CheckIntBetweenValues(this int? input, int minValue, int maxValue, bool required = true)
        {
            if (input == null)
            {
                return !required;
            }

            return (input >= minValue && input <= maxValue);
        }

        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static string ToTime(this int time)
        {
            string hour = $"{time / 60}".PadLeft(2, '0');
            string minutes = $"{time % 60}".PadLeft(2, '0');
            return $"{hour}:{minutes}";
        }

        public static int ToMinutes(this DateTime dateTime)
        {
            return dateTime.Hour * 60 + dateTime.Minute;
        }

        public static Days ToDaysEnum(this DateTime dateTime)
        {
            return (Days)(dateTime.DayOfWeek + 1);
        }

        public static DateTime TrimDate(this DateTime date, DateTimePrecision precision)
        {
            switch (precision)
            {
                case DateTimePrecision.Hour:
                    return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
                case DateTimePrecision.Minute:
                    return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
                case DateTimePrecision.Second:
                    return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                default:
                    return date;
            }
        }
    }
}
