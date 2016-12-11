using System;

namespace Clinic.Common.Core.Dates
{
    /// <summary>
    /// Static class containing Fluent <see cref="DateTime"/> extension methods.
    /// </summary>
    public static class FluentDateTimeExtensionMethods
    {
        /// <summary>
        /// Returns the very end of the given day (the last millisecond of the last hour for the given <see cref="DateTime"/>).
        /// </summary>
        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }
        /// <summary>
        /// Returns the very end of the given day (the last millisecond of the last hour for the given <see cref="DateTime"/>).
        /// </summary>
        public static DateTime? EndOfDay(this DateTime? date)
        {
            if (!date.HasValue) return null;
            return date.Value.EndOfDay();
        }

        /// <summary>
        /// Returns the Start of the given day (the first millisecond of the given <see cref="DateTime"/>).
        /// </summary>
        public static DateTime BeginningOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }
        /// <summary>
        /// Returns the Start of the given day (the first millisecond of the given <see cref="DateTime"/>).
        /// </summary>
        public static DateTime? BeginningOfDay(this DateTime? date)
        {
            if (!date.HasValue) return null;
            return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0, 0);
        }


        /// <summary>
        /// Subtracts given <see cref="TimeSpan"/> from current date (<see cref="DateTime.Now"/>) and returns resulting <see cref="DateTime"/> in the past.
        /// </summary>
        public static DateTime Ago(this TimeSpan from)
        {
            return from.Ago(DateTime.Now);
        }
        public static DateTime Ago(this FluentTimeSpan from)
        {
            return from.Ago(DateTime.Now);
        }
        /// <summary>
        /// Calculates the timespan from target date till this moment
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static TimeSpan Ago(this DateTime from)
        {
            return DomainTime.Now().Subtract(from);
        }

        /// <summary>
        /// Subtracts given <see cref="TimeSpan"/> from <paramref name="originalValue"/> <see cref="DateTime"/> and returns resulting <see cref="DateTime"/> in the past.
        /// </summary>
        public static DateTime Ago(this TimeSpan from, DateTime originalValue)
        {
            return new DateTime((originalValue - from).Ticks);
        }

        /// <summary>
        /// Subtracts given <see cref="TimeSpan"/> from <paramref name="originalValue"/> <see cref="DateTime"/> and returns resulting <see cref="DateTime"/> in the past.
        /// </summary>
        public static DateTime Ago(this FluentTimeSpan from, DateTime originalValue)
        {
            return originalValue.AddMonths(-from.Months).AddYears(-from.Years).Add(-from.TimeSpan);
        }


        /// <summary>
        /// Adds given <see cref="TimeSpan"/> to current <see cref="DateTime.Now"/> and returns resulting <see cref="DateTime"/> in the future.
        /// </summary>
        public static DateTime FromNow(this TimeSpan from)
        {
            return from.From(DateTime.Now);
        }

        /// <summary>
        /// Adds given <see cref="TimeSpan"/> to current <see cref="DateTime.Now"/> and returns resulting <see cref="DateTime"/> in the future.
        /// </summary>
        public static DateTime FromNow(this FluentTimeSpan from)
        {
            return from.From(DateTime.Now);
        }

        /// <summary>
        /// Adds given <see cref="TimeSpan"/> to supplied <paramref name="originalValue"/> <see cref="DateTime"/> and returns resulting <see cref="DateTime"/> in the future.
        /// </summary>
        public static DateTime From(this TimeSpan from, DateTime originalValue)
        {
            return new DateTime((originalValue + from).Ticks);
        }

        /// <summary>
        /// Adds given <see cref="TimeSpan"/> to supplied <paramref name="originalValue"/> <see cref="DateTime"/> and returns resulting <see cref="DateTime"/> in the future.
        /// </summary>
        public static DateTime From(this FluentTimeSpan from, DateTime originalValue)
        {
            return originalValue.AddMonths(from.Months).AddYears(from.Years).Add(from.TimeSpan);
        }

        /// <summary>
        /// Adds given <see cref="TimeSpan"/> to supplied <paramref name="originalValue"/> <see cref="DateTime"/> and returns resulting <see cref="DateTime"/> in the future.
        /// </summary>
        /// <seealso cref="From"/>
        /// <remarks>
        /// Synonym of <see cref="From"/> method.
        /// </remarks>
        public static DateTime Since(this TimeSpan from, DateTime originalValue)
        {
            return From(from, originalValue);
        }

        /// <summary>
        /// Adds given <see cref="TimeSpan"/> to supplied <paramref name="originalValue"/> <see cref="DateTime"/> and returns resulting <see cref="DateTime"/> in the future.
        /// </summary>
        /// <seealso cref="From"/>
        /// <remarks>
        /// Synonym of <see cref="From"/> method.
        /// </remarks>
        public static DateTime Since(this FluentTimeSpan from, DateTime originalValue)
        {
            return From(from, originalValue);
        }


        /// <summary>
        /// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the next calendar year. 
        /// If that day does not exist in next year in same month, number of missing days is added to the last day in same month next year.
        /// </summary>
        public static DateTime NextYear(this DateTime start)
        {
            var nextYear = start.Year + 1;
            var numberOfDaysInSameMonthNextYear = DateTime.DaysInMonth(nextYear, start.Month);

            if (numberOfDaysInSameMonthNextYear < start.Day)
            {
                var differenceInDays = start.Day - numberOfDaysInSameMonthNextYear;
                var dateTime = new DateTime(nextYear, start.Month, numberOfDaysInSameMonthNextYear, start.Hour, start.Minute, start.Second, start.Millisecond);
                return dateTime + differenceInDays.Days();
            }
            return new DateTime(nextYear, start.Month, start.Day, start.Hour, start.Minute, start.Second, start.Millisecond);
        }

        /// <summary>
        /// Returns the same date (same Day, Month, Hour, Minute, Second etc) in the previous calendar year.
        /// If that day does not exist in previous year in same month, number of missing days is added to the last day in same month previous year.
        /// </summary>
        public static DateTime PreviousYear(this DateTime start)
        {
            var previousYear = start.Year - 1;
            var numberOfDaysInSameMonthPreviousYear = DateTime.DaysInMonth(previousYear, start.Month);

            if (numberOfDaysInSameMonthPreviousYear < start.Day)
            {
                var differenceInDays = start.Day - numberOfDaysInSameMonthPreviousYear;
                var dateTime = new DateTime(previousYear, start.Month, numberOfDaysInSameMonthPreviousYear, start.Hour, start.Minute, start.Second, start.Millisecond);
                return dateTime + differenceInDays.Days();
            }
            return new DateTime(previousYear, start.Month, start.Day, start.Hour, start.Minute, start.Second, start.Millisecond);
        }

        public static DateTime PreviousMonth(this DateTime start)
        {
            var year = start.Year;
            var month = start.Month;
            var day = start.Day;
            if (month == 1)
            {
                year = year - 1;
                month = 12;
            }
            else
                month = month - 1;
            var numberOfDaysInPreviousMonth = DateTime.DaysInMonth(year, month);
            if (numberOfDaysInPreviousMonth < day)
            {
                day = numberOfDaysInPreviousMonth;
            }
            return new DateTime(year, month, day, start.Hour, start.Minute, start.Second, start.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> increased by 24 hours ie Next Day.
        /// </summary>
        public static DateTime NextDay(this DateTime start)
        {
            return start + 1.Days();
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> decreased by 24h period ie Previous Day.
        /// </summary>
        public static DateTime PreviousDay(this DateTime start)
        {
            return start - 1.Days();
        }

        /// <summary>
        /// Returns first next occurrence of specified <see cref="DayOfWeek"/>.
        /// </summary>
        public static DateTime Next(this DateTime start, DayOfWeek day)
        {
            do
            {
                start = start.NextDay();
            }
            while (start.DayOfWeek != day);

            return start;
        }

        /// <summary>
        /// Returns first next occurrence of specified <see cref="DayOfWeek"/>.
        /// </summary>
        public static DateTime Previous(this DateTime start, DayOfWeek day)
        {
            do
            {
                start = start.PreviousDay();
            }
            while (start.DayOfWeek != day);

            return start;
        }


        /// <summary>
        /// Increases supplied <see cref="DateTime"/> for 7 days ie returns the Next Week.
        /// </summary>
        public static DateTime WeekAfter(this DateTime start)
        {
            return start + 1.Weeks();
        }

        /// <summary>
        /// Decreases supplied <see cref="DateTime"/> for 7 days ie returns the Previous Week.
        /// </summary>
        public static DateTime WeekEarlier(this DateTime start)
        {
            return start - 1.Weeks();
        }
        public static DateTime? WeekEarlier(this DateTime? start)
        {
            if (start.HasValue) return start.Value.WeekEarlier();
            return null;
        }
        /// <summary>
        /// Generates <see cref="TimeSpan"/> value for given number of Years.
        /// </summary>
        public static FluentTimeSpan Years(this int years)
        {
            return new FluentTimeSpan { Years = years };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> value for given number of Months.
        /// </summary>
        public static FluentTimeSpan Months(this int months)
        {
            return new FluentTimeSpan { Months = months };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Weeks (number of weeks * 7).
        /// </summary>
        public static FluentTimeSpan Weeks(this int weeks)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromDays(weeks * 7) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Weeks (number of weeks * 7).
        /// </summary>
        public static FluentTimeSpan Weeks(this double weeks)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromDays(weeks * 7) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Days.
        /// </summary>
        public static FluentTimeSpan Days(this int days)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromDays(days) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Days.
        /// </summary>
        public static FluentTimeSpan Days(this double days)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromDays(days) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Hours.
        /// </summary>
        public static FluentTimeSpan Hours(this int hours)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromHours(hours) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Hours.
        /// </summary>
        public static FluentTimeSpan Hours(this double hours)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromHours(hours) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Minutes.
        /// </summary>
        public static FluentTimeSpan Minutes(this int minutes)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromMinutes(minutes) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Minutes.
        /// </summary>
        public static FluentTimeSpan Minutes(this double minutes)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromMinutes(minutes) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Seconds.
        /// </summary>
        public static FluentTimeSpan Seconds(this int seconds)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromSeconds(seconds) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Seconds.
        /// </summary>
        public static FluentTimeSpan Seconds(this double seconds)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromSeconds(seconds) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Milliseconds.
        /// </summary>
        public static FluentTimeSpan Milliseconds(this int milliseconds)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromMilliseconds(milliseconds) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of Milliseconds.
        /// </summary>
        public static FluentTimeSpan Milliseconds(this double milliseconds)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromMilliseconds(milliseconds) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of ticks.
        /// </summary>
        public static FluentTimeSpan Ticks(this int ticks)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromTicks(ticks) };
        }

        /// <summary>
        /// Returns <see cref="TimeSpan"/> for given number of ticks.
        /// </summary>
        public static FluentTimeSpan Ticks(this long ticks)
        {
            return new FluentTimeSpan { TimeSpan = TimeSpan.FromTicks(ticks) };
        }



        /// <summary>
        /// Increases the <see cref="DateTime"/> object with given <see cref="TimeSpan"/> value.
        /// </summary>
        public static DateTime IncreaseTime(this DateTime startDate, TimeSpan toAdd)
        {
            return startDate + toAdd;
        }

        /// <summary>
        /// Decreases the <see cref="DateTime"/> object with given <see cref="TimeSpan"/> value.
        /// </summary>
        public static DateTime DecreaseTime(this DateTime startDate, TimeSpan toSubtract)
        {
            return startDate - toSubtract;
        }

        /// <summary>
        /// Returns the original <see cref="DateTime"/> with Hour part changed to supplied hour parameter.
        /// </summary>
        public static DateTime SetTime(this DateTime originalDate, int hour)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, originalDate.Minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns the original <see cref="DateTime"/> with Hour and Minute parts changed to supplied hour and minute parameters.
        /// </summary>
        public static DateTime SetTime(this DateTime originalDate, int hour, int minute)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns the original <see cref="DateTime"/> with Hour, Minute and Second parts changed to supplied hour, minute and second parameters.
        /// </summary>
        public static DateTime SetTime(this DateTime originalDate, int hour, int minute, int second)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns the original <see cref="DateTime"/> with Hour, Minute, Second and Millisecond parts changed to supplied hour, minute, second and millisecond parameters.
        /// </summary>
        public static DateTime SetTime(this DateTime originalDate, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, minute, second, millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Hour part.
        /// </summary>
        public static DateTime SetHour(this DateTime originalDate, int hour)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, hour, originalDate.Minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Minute part.
        /// </summary>
        public static DateTime SetMinute(this DateTime originalDate, int minute)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, minute, originalDate.Second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Second part.
        /// </summary>
        public static DateTime SetSecond(this DateTime originalDate, int second)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, originalDate.Minute, second, originalDate.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Millisecond part.
        /// </summary>
        public static DateTime SetMillisecond(this DateTime originalDate, int millisecond)
        {
            return new DateTime(originalDate.Year, originalDate.Month, originalDate.Day, originalDate.Hour, originalDate.Minute, originalDate.Second, millisecond);
        }

        /// <summary>
        /// Returns original <see cref="DateTime"/> value with time part set to midnight (alias for <see cref="BeginningOfDay"/> method).
        /// </summary>
        public static DateTime Midnight(this DateTime value)
        {
            return value.BeginningOfDay();
        }

        /// <summary>
        /// Returns original <see cref="DateTime"/> value with time part set to Noon (12:00:00h).
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> find Noon for.</param>
        /// <returns>A <see cref="DateTime"/> value with time part set to Noon (12:00:00h).</returns>
        public static DateTime Noon(this DateTime value)
        {
            return value.SetTime(12, 0, 0, 0);
        }

        public static bool IsBeforeNoon(this DateTime value)
        {
            return value.TimeOfDay < new TimeSpan(12, 0, 0);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Year part.
        /// </summary>
        public static DateTime SetDate(this DateTime value, int year)
        {
            return new DateTime(year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Year and Month part.
        /// </summary>
        public static DateTime SetDate(this DateTime value, int year, int month)
        {
            return new DateTime(year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Year, Month and Day part.
        /// </summary>
        public static DateTime SetDate(this DateTime value, int year, int month, int day)
        {
            return new DateTime(year, month, day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Year part.
        /// </summary>
        public static DateTime SetYear(this DateTime value, int year)
        {
            return new DateTime(year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Month part.
        /// </summary>
        public static DateTime SetMonth(this DateTime value, int month)
        {
            return new DateTime(value.Year, month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Returns <see cref="DateTime"/> with changed Day part.
        /// </summary>
        public static DateTime SetDay(this DateTime value, int day)
        {
            return new DateTime(value.Year, value.Month, day, value.Hour, value.Minute, value.Second, value.Millisecond);
        }

        /// <summary>
        /// Determines whether the specified <see cref="DateTime"/> is before then current value.
        /// </summary>
        /// <param name="current">The current value.</param>
        /// <param name="toCompareWith">Value to compare with.</param>
        /// <returns>
        /// 	<c>true</c> if the specified current is before; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBefore(this DateTime current, DateTime toCompareWith)
        {
            return current < toCompareWith;
        }

        /// <summary>
        /// Determines whether the specified <see cref="DateTime"/> value is After then current value.
        /// </summary>
        /// <param name="current">The current value.</param>
        /// <param name="toCompareWith">Value to compare with.</param>
        /// <returns>
        /// 	<c>true</c> if the specified current is after; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAfter(this DateTime current, DateTime toCompareWith)
        {
            return current > toCompareWith;
        }

        /// <summary>
        /// Returns the given <see cref="DateTime"/> with hour and minutes set At given values.
        /// </summary>
        /// <param name="current">The current <see cref="DateTime"/> to be changed.</param>
        /// <param name="hour">The hour to set time to.</param>
        /// <param name="minute">The minute to set time to.</param>
        /// <returns><see cref="DateTime"/> with hour and minute set to given values.</returns>
        public static DateTime At(this DateTime current, int hour, int minute)
        {
            return current.SetTime(hour, minute);
        }

        /// <summary>
        /// Returns the given <see cref="DateTime"/> with hour and minutes and seconds set At given values.
        /// </summary>
        /// <param name="current">The current <see cref="DateTime"/> to be changed.</param>
        /// <param name="hour">The hour to set time to.</param>
        /// <param name="minute">The minute to set time to.</param>
        /// <param name="second">The second to set time to.</param>
        /// <returns><see cref="DateTime"/> with hour and minutes and seconds set to given values.</returns>
        public static DateTime At(this DateTime current, int hour, int minute, int second)
        {
            return current.SetTime(hour, minute, second);
        }

        /// <summary>
        /// Sets the day of the <see cref="DateTime"/> to the first day in that month.
        /// </summary>
        /// <param name="current">The current <see cref="DateTime"/> to be changed.</param>
        /// <returns>given <see cref="DateTime"/> with the day part set to the first day in that month.</returns>
        public static DateTime FirstDayOfMonth(this DateTime current)
        {
            return current.SetDay(1);
        }

        /// <summary>
        /// Sets the day of the <see cref="DateTime"/> to the last day in that month.
        /// </summary>
        /// <param name="current">The current DateTime to be changed.</param>
        /// <returns>given <see cref="DateTime"/> with the day part set to the last day in that month.</returns>
        public static DateTime LastDayOfMonth(this DateTime current)
        {
            return current.SetDay(DateTime.DaysInMonth(current.Year, current.Month));
        }


        /// <summary>
        /// Adds the given number of business days to the <see cref="DateTime"/>.
        /// </summary>
        /// <param name="current">The date to be changed.</param>
        /// <param name="days">Number of business days to be added.</param>
        /// <returns>A <see cref="DateTime"/> increased by a given number of business days.</returns>
        public static DateTime AddBusinessDays(this DateTime current, int days)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    current = current.AddDays(sign);
                }
                while (current.DayOfWeek == DayOfWeek.Saturday ||
                    current.DayOfWeek == DayOfWeek.Sunday);
            }
            return current;
        }
        public static DateTime AddWeeks(this DateTime current, int weeks)
        {
            return current.AddDays(7 * weeks);
        }

        /// <summary>
        /// Subtracts the given number of business days to the <see cref="DateTime"/>.
        /// </summary>
        /// <param name="current">The date to be changed.</param>
        /// <param name="days">Number of business days to be subtracted.</param>
        /// <returns>A <see cref="DateTime"/> increased by a given number of business days.</returns>
        public static DateTime SubtractBusinessDays(this DateTime current, int days)
        {
            return AddBusinessDays(current, -days);
        }


        /// <summary>
        /// Determine if a <see cref="DateTime"/> is in the future.
        /// </summary>
        /// <param name="dateTime">The date to be checked.</param>
        /// <returns><c>true</c> if <paramref name="dateTime"/> is in the future; otherwise <c>false</c>.</returns>
        public static bool IsInFuture(this DateTime dateTime)
        {
            return dateTime > DateTime.Now;
        }


        /// <summary>
        /// Determine if a <see cref="DateTime"/> is in the past.
        /// </summary>
        /// <param name="dateTime">The date to be checked.</param>
        /// <returns><c>true</c> if <paramref name="dateTime"/> is in the past; otherwise <c>false</c>.</returns>
        public static bool IsInPast(this DateTime dateTime)
        {
            return dateTime < DateTime.Now;
        }


        /// <summary>
        /// Convert a <see cref="TimeSpan"/> to a human readable string.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> to convert</param>
        /// <returns>A human readable string for <paramref name="timeSpan"/></returns>
        public static string ToDisplayString(this FluentTimeSpan timeSpan)
        {
            return ((TimeSpan)timeSpan).ToDisplayString();
        }

        /// <summary>
        /// Convert a <see cref="TimeSpan"/> to a human readable string.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> to convert</param>
        /// <returns>A human readable string for <paramref name="timeSpan"/></returns>
        public static string ToDisplayString(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays > 1)
            {
                var round = timeSpan.Round(RoundTo.Hour);

                return string.Format("{0} {1} {2} {3}", round.Days.FormatDays(), Resources.Strings.And, round.Hours, Resources.Strings.Hour_Short);
            }
            if (timeSpan.TotalHours > 1)
            {
                var round = timeSpan.Round(RoundTo.Minute);
                return string.Format("{0} {1} {2} {3} {4}", round.Hours, Resources.Strings.Hour_Short, Resources.Strings.And, round.Minutes, Resources.Strings.Minute_Short);
            }
            if (timeSpan.TotalMinutes > 1)
            {
                var round = timeSpan.Round(RoundTo.Second);
                return string.Format("{0} {1} {2} {3} {4}", round.Minutes, Resources.Strings.Minute_Short, Resources.Strings.And, round.Seconds, Resources.Strings.Seconds_Short);
            }
            if (timeSpan.TotalSeconds > 1)
            {
                return string.Format("{0} {1}", timeSpan.TotalSeconds, Resources.Strings.Seconds_Short);
            }
            return string.Format("{0} {1}", timeSpan.Milliseconds, Resources.Strings.Milliseconds);
        }

        public static DateTime Round(this DateTime dateTime, RoundTo rt)
        {
            DateTime rounded;

            switch (rt)
            {
                case RoundTo.Second:
                    {
                        rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
                        if (dateTime.Millisecond >= 500)
                        {
                            rounded = rounded.AddSeconds(1);
                        }
                        break;
                    }
                case RoundTo.Minute:
                    {
                        rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
                        if (dateTime.Second >= 30)
                        {
                            rounded = rounded.AddMinutes(1);
                        }
                        break;
                    }
                case RoundTo.Hour:
                    {
                        rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
                        if (dateTime.Minute >= 30)
                        {
                            rounded = rounded.AddHours(1);
                        }
                        break;
                    }
                case RoundTo.Day:
                    {
                        rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
                        if (dateTime.Hour >= 12)
                        {
                            rounded = rounded.AddDays(1);
                        }
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }

            return rounded;
        }

        public static TimeSpan Round(this TimeSpan timeSpan, RoundTo rt)
        {
            TimeSpan rounded;

            switch (rt)
            {
                case RoundTo.Second:
                    {
                        rounded = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                        if (timeSpan.Milliseconds >= 500)
                        {
                            rounded = rounded + 1.Seconds();
                        }
                        break;
                    }
                case RoundTo.Minute:
                    {
                        rounded = new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, 0);
                        if (timeSpan.Seconds >= 30)
                        {
                            rounded = rounded + 1.Minutes();
                        }
                        break;
                    }
                case RoundTo.Hour:
                    {
                        rounded = new TimeSpan(timeSpan.Days, timeSpan.Hours, 0, 0);
                        if (timeSpan.Minutes >= 30)
                        {
                            rounded = rounded + 1.Hours();
                        }
                        break;
                    }
                case RoundTo.Day:
                    {
                        rounded = new TimeSpan(timeSpan.Days, 0, 0, 0);
                        if (timeSpan.Hours >= 12)
                        {
                            rounded = rounded + 1.Days();
                        }
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }

            return rounded;
        }

        public enum RoundTo
        {
            Second, Minute, Hour, Day
        }

        /// <summary>
        /// True if both dates have same day and month values 
        /// </summary>
        /// <returns></returns>
        public static bool SameBirthday(this DateTime? myDate, DateTime? birthdate)
        {
            if (!birthdate.HasValue) return false;
            return myDate.Value.SameBirthday(birthdate.Value);
        }
        /// <summary>
        /// True if both dates have same day and month values 
        /// </summary>
        public static bool SameBirthday(this DateTime myDate, DateTime birthdate)
        {
            return myDate.Month == birthdate.Month && myDate.Day == birthdate.Day;
        }

        /// <summary>
        /// Substracts two dates 
        /// </summary>
        /// <returns>Returns a Year, Month, Day time span</returns>
        public static FluentTimeSpan SubtractFrom(this DateTime thisDate, DateTime targetDate)
        {
            //DateTime startDate;
            //DateTime endDate;
            //if (thisDate > targetDate)
            //{
            //    startDate = targetDate;
            //    endDate = thisDate;
            //}
            //else
            //{
            //    startDate = thisDate;
            //    endDate = targetDate;

            //}

            //int iYear = (endDate.Year - startDate.Year);
            //if (endDate.Year != startDate.Year)
            //    if (endDate.Month < startDate.Month)
            //        iYear = iYear - 1;
            //    else if (endDate.Month == startDate.Month)
            //        if (endDate.Day < startDate.Day)
            //            iYear = iYear - 1;

            //FluentTimeSpan time = new FluentTimeSpan();
            //time.Years = iYear;

            //return time;
            var difference = new DateDifference(thisDate, targetDate);
            var timespan = new FluentTimeSpan();
            timespan.Years = difference.Years;
            timespan.Months = difference.Months;
            timespan.TimeSpan = new TimeSpan(difference.Days, 0, 0);

            return timespan;
        }

        //TODO: equality tests: DateIsEqual() TimeIsEqual() 
    }
}
