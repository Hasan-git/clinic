using System;
using Clinic.Common.Resources;

namespace Clinic.Common.Core.Dates
{
    public static class TimeExtensions
    {
        /// <summary>
        /// Returns true if this date was in future from UTC now
        /// </summary>
        /// <param name="current">The current UTC date</param>
        public static bool IsFutureDate(this DateTime current)
        {
            return current.CompareTo(DomainTime.Now().Midnight().AddDays(1)) >= 0;
        }

        public static bool IsToday(this DateTime current)
        {
            return current.Midnight().CompareTo(DomainTime.Now().Midnight()) == 0;
        }
        public static bool IsToday(this DateTime? current)
        {
            return current.HasValue && current.Value.Midnight().CompareTo(DomainTime.Now().Midnight()) == 0;
        }

        /// <summary>
        /// Returns true if first date was earlier than second date
        /// </summary>
        /// <param name="current">First date</param>
        /// <param name="date">Second date</param>
        public static bool IsEarlierThan(this DateTime current, DateTime date)
        {
            return current.ToUniversalTime().CompareTo(date.ToUniversalTime()) < 0;
        }

        /// <summary>
        /// Returns day of week format string value 
        /// </summary>
        public static string ToDayOfWeekFormat(this DateTime? date, string lang)
        {
            return date.HasValue ? date.Value.ToDayOfWeekFormat(lang) : "";
        }
        public static string ToDayOfWeekFormat(this DateTime date, string lang)
        {
            return date.ToDayOfWeekFormat(false, lang);
        }
        public static string ToDayOfWeekFormat(this DateTime? date, bool withTime, string lang)
        {
            return date.HasValue ? date.Value.ToDayOfWeekFormat(withTime, lang) : "";
        }

        public static string ToDayOfWeekFormat(this DateTime date, bool withTime, string lang)
        {
            var time = withTime ? ("HH:mm") : "";
            var format = string.Concat("dddd ", time);
            return date.ToString(format, new System.Globalization.CultureInfo(lang));
        }
        /// <summary>
        /// Returns yyyy-MM-dd format string value
        /// </summary>
        public static string ToStandardFormat(this DateTime? date)
        {
            return date.HasValue ? date.Value.ToStandardFormat() : "";
        }
        /// <summary>
        /// Returns yyyy-MM-dd format string value but no time.
        /// </summary>
        public static string ToStandardFormat(this DateTime date)
        {
            return date.ToStandardFormat(false, false);
        }
        public static string ToStandardFormat(this DateTime date, bool withTime)
        {
            return date.ToStandardFormat(withTime, false);
        }
        /// <summary>
        /// Returns yyyy-MM-dd format string value with time as HH:mm:ss format
        /// </summary>
        public static string ToStandardFormat(this DateTime? date, bool withTime)
        {
            return date.HasValue ? date.Value.ToStandardFormat(withTime, false) : "";
        }

        /// <summary>
        /// Returns yyyy-MM-dd format string value with time as HH:mm:ss format
        /// </summary>
        public static string ToStandardFormat(this DateTime? date, bool withTime, bool withSeconds)
        {
            return date.HasValue ? date.Value.ToStandardFormat(withTime, withSeconds) : "";
        }

        public static string ToSQLFormat(this DateTime? date)
        {
            return date.HasValue ? "'" + date.Value.ToStandardFormat(true, true) + "'" : "NULL";
        }

        /// <summary>
        /// Returns yyyy-MM-dd format string value with time as HH:mm:ss format
        /// </summary>
        public static string ToStandardFormat(this DateTime date, bool withTime, bool withSeconds)
        {
            var time = withTime ? (withSeconds ? " HH:mm:ss" : " HH:mm") : "";
            var format = string.Concat("yyyy-MM-dd", time);
            return date.ToString(format, System.Globalization.CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// Returns for example '13 Jan 2010'
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToNamedFormat(this DateTime? date)
        {
            return ToNamedFormat(date, " ");
        }
        /// <summary>
        /// Returns dd MMM yyyy format string value
        /// </summary>
        public static string ToNamedFormat(this DateTime? date, string splitter)
        {
            var emtpyDate = "---------";
            if (date.HasValue)
                return date.Value.ToNamedFormat();
            return emtpyDate;
        }
        /// <summary>
        /// Returns dd MMM yyyy format string value
        /// </summary>
        public static string ToNamedFormat(this DateTime date)
        {
            return ToNamedFormat(date, " ");
        }
        public static string ToNamedFormat(this DateTime date, string splitter)
        {
            return ToNamedFormat(date, true, splitter);
        }

        public static string ToNamedFormat(this DateTime date, bool showAll, string splitter = " ")
        {
            var emtpyDate = "";
            var monthName = "MMM";
            monthName = GetMonthName(date.Month);
            var timePeriod = GetTimePeriod(date);
            var format = string.Concat("d", splitter, monthName);
            var formatted = string.Concat(date.Day, splitter, monthName);

            if (date.Date == DomainTime.Now().Date)
            {
                //    return string.Concat(Strings.Today, splitter, date.ToString("hh:mm:ss "), timePeriod);
                return string.Concat(Strings.Today, splitter, date.ToString("hh:mm "), timePeriod);
            }
            else
            {
                if (showAll || date.Year != DomainTime.Now().Year)
                {
                    //format = string.Concat(format, splitter, "yyyy");
                    formatted = string.Concat(formatted, splitter, date.ToString("yyyy"));
                }

                if (date != date.Midnight())
                {
                    //format = string.Concat(format, ",", splitter, "hh:mm:ss ", timePeriod);
                    // formatted = string.Concat(formatted, ",", splitter, date.ToString("hh:mm:ss "), timePeriod);
                    formatted = string.Concat(formatted, ",", splitter, date.ToString("hh:mm"), timePeriod);

                }

                return date == DateTime.MinValue ? emtpyDate :
                    //date.ToString(format, System.Globalization.CultureInfo.InvariantCulture)
                    formatted
                    ;
            }
        }

        private static string GetTimePeriod(DateTime date)
        {
            return date.IsBeforeNoon() ? Strings.AM : Strings.PM;
        }

        private static string GetMonthName(int month)
        {
            var name = "MMM";
            switch (month)
            {
                case 1:
                    name = Strings.Jan;
                    break;
                case 2:
                    name = Strings.Feb;
                    break;
                case 3:
                    name = Strings.Mar;
                    break;
                case 4:
                    name = Strings.Apr;
                    break;
                case 5:
                    name = Strings.May;
                    break;
                case 6:
                    name = Strings.Jun;
                    break;
                case 7:
                    name = Strings.Jul;
                    break;
                case 8:
                    name = Strings.Aug;
                    break;
                case 9:
                    name = Strings.Sep;
                    break;
                case 10:
                    name = Strings.Oct;
                    break;
                case 11:
                    name = Strings.Nov;
                    break;
                case 12:
                    name = Strings.Dec;
                    break;
            }
            return name;
        }

        public static string ToFormat(this DateTime date, string format)
        {
            return date.ToString(format, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns dd MMM yyyy format string value
        /// </summary>
        public static string ToSmartFormat(this DateTime? date, int numOfDays = 1)
        {
            var emtpyDate = Resources.Strings.Unknown;// "Unknown";
            if (date.HasValue && date.Value < DomainTime.Now())
                return date.Value.ToSmartFormat(numOfDays);
            return emtpyDate;
        }
        public static string ToSmartFormat(this DateTime date, int numOfDays = 1)
        {
            var emtpyDate = "---------";
            var splitter = " ";

            //var format = string.Concat("d", splitter, "MMM");

            var monthName = "MMM";
            monthName = GetMonthName(date.Month);
            var timePeriod = GetTimePeriod(date);
            //var format = string.Concat("d", splitter, monthName);
            var formatted = string.Concat(date.Day, splitter, monthName);

            if (date.Date >= DomainTime.Now().Date.AddDays(-numOfDays))
            {
                var timespan = date.Ago();
                return timespan.GetTimeInfo();
            }
            if (date.Year != DomainTime.Now().Year)
            {
                //format = string.Concat(format, splitter, "yyyy");
                formatted = string.Concat(formatted, splitter, date.ToString("yyyy"));
            }

            if (date != date.Midnight())
            {
                //format = string.Concat(format, ",", splitter, "hh:mm ", timePeriod);
                formatted = string.Concat(formatted, ",", splitter, date.ToString("hh:mm "), timePeriod);
            }

            return date == DateTime.MinValue
                       ? emtpyDate
                       : //date.ToString(format, System.Globalization.CultureInfo.InvariantCulture);
                   formatted;
        }

        public static string GetTimeInfo(this TimeSpan timespan)
        {
            var message = "";
            if (timespan.TotalMinutes <= 1)
                message = Strings.FewSeconds;// "Few seconds";
            else if (timespan.TotalMinutes <= 5)
                message = "5 " + Strings.Minute_Short;//"min.";
            else if (timespan.TotalMinutes <= 10)
                message = "10 " + Strings.Minute_Short;//min.";
            else if (timespan.TotalMinutes <= 15)
                message = "15 " + Strings.Minute_Short;//min.";
            else if (timespan.TotalMinutes <= 20)
                message = "20 " + Strings.Minute_Short;//min.";
            else if (timespan.TotalMinutes <= 25)
                message = "25 " + Strings.Minute_Short;//min.";
            else if (timespan.TotalMinutes <= 30)
                message = "30 " + Strings.Minute_Short;//min.";
            else if (timespan.TotalMinutes <= 40)
                message = "40 " + Strings.Minute_Short;//min.";
            else if (timespan.TotalMinutes <= 50)
                message = "50 " + Strings.Minute_Short;//min.";
            else if (timespan.TotalMinutes <= 60)
                message = "1 " + Strings.Hour_Short;
            else if (timespan.TotalMinutes <= 90)
                message = "1.5 " + Strings.Hour_Short;
            else if (timespan.TotalMinutes <= 120)
                message = "2 " + Strings.Hour_Short;
            else if (timespan.TotalMinutes <= 150)
                message = "2.5 " + Strings.Hour_Short;
            else if (timespan.TotalMinutes <= 180)
                message = Strings.About + " 3 " + Strings.Hour_Short;
            else if (timespan.TotalHours >= 3 && timespan.TotalHours < 23)
                message = string.Concat(Strings.About, " ", Math.Ceiling(timespan.TotalHours), " " + Strings.Hour_Short);
            else if (timespan.TotalHours >= 23 && timespan.TotalHours < 47)
                message = Strings.About + " " + Strings.Day;
            else if (timespan.TotalHours >= 47)
                message = timespan.TotalDays.FormatDays();

            return message;
        }

        public static string FormatDays(this int value)
        {
            return ((double)value).FormatDays();
        }

        public static string FormatDays(this double value)
        {
            value = Math.Round(value);
            var message = "";
            if (value >= 1 && value < 2)
                message = Strings.About + " " + Strings.Day;
            else if (value >= 2 && value < 3)
                message = Strings.About + " " + Strings.TwoDays;
            else if (value >= 3 && value < 11)
                message = Strings.About + " " + value + " " + Strings.Days;
            else if (value >= 11)
                message = Strings.About + " " + value + " " + Strings.Day;

            return message;
        }

        public static bool EqualsTo(this DateTime? expected, DateTime? target)
        {
            return expected.EqualsTo(target, TimeSpan.FromSeconds(1));
        }
        public static bool EqualsTo(this DateTime? expected, DateTime? target, TimeSpan tolerance)
        {
            if (expected == null || target == null) return false;
            return expected.Value.EqualsTo(target.Value, tolerance);
        }

        public static bool EqualsTo(this DateTime expected, DateTime target, TimeSpan tolerance)
        {
            return ((DateTime)expected - (DateTime)target).Duration() <= (TimeSpan)tolerance;
        }

        public static DateTime? ToUTC(this DateTime? date)
        {
            if (date.HasValue) return date.Value.ToUTC();
            return null;
        }
        public static DateTime ToUTC(this DateTime date)
        {
            return date.ToUniversalTime();
        }

        public static long ToUnixTimespan(this DateTime date)
        {
            TimeSpan tspan = date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)Math.Truncate(tspan.TotalSeconds);
        }
        public static DateTime FromUnixTimestamp(this double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public static DateTime AddTime(this DateTime date, TimeSpan time)
        {
            return date.Add(time);
        }

        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }

}

