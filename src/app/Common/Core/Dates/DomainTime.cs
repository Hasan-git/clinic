using System;
using System.Globalization;

namespace Clinic.Common.Core.Dates
{
    public static class DomainTime
    {
        public static Func<DateTime> Now = () => DateTime.UtcNow;                                               

        //public static DateTime? Parse(string date)
        //{
        //    if (string.IsNullOrEmpty(date))
        //        return null;
        //    DateTime validDate;
        //    if (DateTime.TryParse(date, out validDate)) return validDate;
        //    return null;
        //}

        public static DateTime? Parse(string date, string format = "yyyy-MM-dd")
        {
            if (string.IsNullOrEmpty(date))
                return null;
            try
            {
                return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
            }
            catch
            { }

            return null;
        }       
    }
}
