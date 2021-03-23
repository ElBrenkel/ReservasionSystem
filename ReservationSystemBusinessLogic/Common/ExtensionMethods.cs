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

        public static DateTime ToLocalDate(this DateTime date)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.Local);
        }

        public static int LevenshteinDistance(this string a, string b)
        {
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
            {
                return 0;
            }
            if (String.IsNullOrEmpty(a))
            {
                return b.Length;
            }
            if (String.IsNullOrEmpty(b))
            {
                return a.Length;
            }
            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            return distances[lengthA, lengthB];
        }
    }
}
