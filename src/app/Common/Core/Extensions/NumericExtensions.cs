using System;

namespace Clinic.Common.Core.Extensions
{
    public static class NumericExtensions
    {
        public static decimal ToDecimal(this double value)
        {
            return Convert.ToDecimal(value);
        }
        public static double ToDouble(this decimal value)
        {
            return Convert.ToDouble(value);
        }
        public static int ToInteger(this double value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// For example 565 becomes "Five hundred and sixty-five"
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToWords(this int number)
        {
            return NumberToWords.ConvertToWords(number);
        }
        /// <summary>
        /// For example 90,000 becomes 90K and 5,600,000 becomes 5.6M
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToSymbol(this int number)
        {
            return NumberToWords.ConvertToSymbol(number);
        }

        /// <summary>
        /// For example 90,000 becomes 90K and 5,600,000 becomes 5.6M
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToSymbol(this long number)
        {
            return NumberToWords.ConvertToSymbol(number);
        }

        /// <summary>
        /// For example 90,000 becomes 90K and 5,600,000 becomes 5.6M
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToSymbol(this double number)
        {
            return NumberToWords.ConvertToSymbol((decimal)number);
        }

        /// <summary>
        /// For example 90,000 becomes 90K and 5,600,000 becomes 5.6M
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToSymbol(this decimal number)
        {
            return NumberToWords.ConvertToSymbol(number);
        }

        public static string ToSigned(this decimal number)
        {
            if (number == decimal.Zero) return "0";
            var sign = number > 0 ? "+" : "";
            return string.Concat(sign, number.ToString());
        }
    }
}
