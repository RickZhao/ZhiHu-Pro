using System;
using System.Globalization;

using Windows.UI.Xaml.Data;

using Zhihu.Common.Helper;


namespace Zhihu.Converter
{
    /// <summary>
    /// Time converter to display elapsed time relatively to the present.
    /// </summary>
    internal sealed class RelativeTimeConverter : IValueConverter
    {
        /// <summary>
        /// A minute defined in seconds.
        /// </summary>
        private const double Minute = 60.0;

        /// <summary>
        /// An hour defined in seconds.
        /// </summary>
        private const double Hour = 60.0 * Minute;

        /// <summary>
        /// A day defined in seconds.
        /// </summary>
        private const double Day = 24 * Hour;

        /// <summary>
        /// A week defined in seconds.
        /// </summary>
        private const double Week = 7 * Day;

        /// <summary>
        /// A month defined in seconds.
        /// </summary>
        private const double Month = 30.5 * Day;

        /// <summary>
        /// A year defined in seconds.
        /// </summary>
        private const double Year = 365 * Day;

        /// <summary>
        /// Abbreviation for the default culture used by resources files.
        /// </summary>
        private const String DefaultCulture = "en-US";

        /// <summary>
        /// Four different strings to express hours in plural.
        /// </summary>
        private String[] PluralHourStrings;

        /// <summary>
        /// Four different strings to express minutes in plural.
        /// </summary>
        private String[] PluralMinuteStrings;

        /// <summary>
        /// Four different strings to express seconds in plural.
        /// </summary>
        private String[] PluralSecondStrings;

        /// <summary>
        /// Resources use the culture in the system locale by default.
        /// The converter must use the culture specified the ConverterCulture.
        /// The ConverterCulture defaults to en-US when not specified.
        /// Thus, change the resources culture only if ConverterCulture is set.
        /// </summary>
        /// <param name="culture">The culture to use in the converter.</param>
        private void SetLocalizationCulture()
        {
            PluralHourStrings = new String[4]
            {
                "{0} 小时前",
                "{0} 小时前",
                "{0} 小时前",
                "{0} 小时前"
            };

            PluralMinuteStrings = new String[4]
            {
                "{0} 分钟前", 
                "{0} 分钟前", 
                "{0} 分钟前", 
                "{0} 分钟前"
            };

            PluralSecondStrings = new String[4]
            {
                "{0} 秒前",
                "{0} 秒前",
                "{0} 秒前",
                "{0} 秒前"
            };
        }

        /// <summary>
        /// Returns a localized text String to express months in plural.
        /// </summary>
        /// <param name="month">Number of months.</param>
        /// <returns>Localized text String.</returns>
        private static String GetPluralMonth(Int32 month)
        {
            if (month >= 2 && month <= 4)
            {
                return String.Format(CultureInfo.CurrentUICulture, "{0} 个月前", month.ToString());
            }
            else if (month >= 5 && month <= 12)
            {
                return String.Format(CultureInfo.CurrentUICulture, "{0} 个月前", month.ToString());
            }
            else
            {
                throw new ArgumentException("Invalid argument. The number of months should be greater than 1 and less than 12.");
            }
        }

        /// <summary>
        /// Returns a localized text String to express time units in plural.
        /// </summary>
        /// <param name="units">
        /// Number of time units, e.g. 5 for five months.
        /// </param>
        /// <param name="resources">
        /// Resources related to the specified time unit.
        /// </param>
        /// <returns>Localized text String.</returns>
        private static String GetPluralTimeUnits(Int32 units, String[] resources)
        {
            Int32 modTen = units % 10;
            Int32 modHundred = units % 100;

            if (units <= 1)
            {
                throw new ArgumentException("Invalid argument. The number of time units should be greater than 1.");
            }
            else if (units >= 2 && units <= 4)
            {
                return String.Format(CultureInfo.CurrentUICulture, resources[0], units.ToString());
            }
            else if (modTen == 1 && modHundred != 11)
            {
                return String.Format(CultureInfo.CurrentUICulture, resources[1], units.ToString());
            }
            else if ((modTen >= 2 && modTen <= 4) && !(modHundred >= 12 && modHundred <= 14))
            {
                return String.Format(CultureInfo.CurrentUICulture, resources[2], units.ToString());
            }
            else
            {
                return String.Format(CultureInfo.CurrentUICulture, resources[3], units.ToString());
            }
        }

        /// <summary>
        /// Returns a localized text String for the "ast" + "day of week as {0}".
        /// </summary>
        /// <param name="dow">Last Day of week.</param>
        /// <returns>Localized text String.</returns>
        private static String GetLastDayOfWeek(DayOfWeek dow)
        {
            String result;

            switch (dow)
            {
                case DayOfWeek.Monday:
                    result = "上个星期一";
                    break;
                case DayOfWeek.Tuesday:
                    result = "上星期二";
                    break;
                case DayOfWeek.Wednesday:
                    result = "上星期三";
                    break;
                case DayOfWeek.Thursday:
                    result = "上星期四";
                    break;
                case DayOfWeek.Friday:
                    result = "上星期五";
                    break;
                case DayOfWeek.Saturday:
                    result = "上星期六";
                    break;
                case DayOfWeek.Sunday:
                    result = "上个星期日";
                    break;
                default:
                    result = "上个星期日";
                    break;
            }

            return result;
        }


        /// <summary>
        /// Returns a localized text String to express "on {0}"
        /// where {0} is a day of the week, e.g. Sunday.
        /// </summary>
        /// <param name="dow">Day of week.</param>
        /// <returns>Localized text String.</returns>
        private static String GetOnDayOfWeek(DayOfWeek dow)
        {
            String result;

            switch (dow)
            {
                case DayOfWeek.Monday:
                    result = "星期一";
                    break;
                case DayOfWeek.Tuesday:
                    result = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    result = "星期三";
                    break;
                case DayOfWeek.Thursday:
                    result = "星期四";
                    break;
                case DayOfWeek.Friday:
                    result = "星期五";
                    break;
                case DayOfWeek.Saturday:
                    result = "星期六";
                    break;
                case DayOfWeek.Sunday:
                    result = "星期日";
                    break;
                default:
                    result = "星期日";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Converts a 
        /// <see cref="T:System.DateTime"/>
        /// object into a String the represents the elapsed time 
        /// relatively to the present.
        /// </summary>
        /// <param name="value">The given date and time.</param>
        /// <param name="targetType">
        /// The type corresponding to the binding property, which must be of
        /// <see cref="T:System.String"/>.
        /// </param>
        /// <param name="parameter">(Not used).</param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// When not specified, the converter uses the current culture
        /// as specified by the system locale.
        /// </param>
        /// <returns>The given date and time as a String.</returns>
        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            var timeStamp = (Int32)value;

            var unixDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            unixDateTime = unixDateTime.AddSeconds(timeStamp);

            unixDateTime = unixDateTime.ToLocalTime();

            // Target value must be a System.DateTime object.
            //if (!(value is DateTime))
            //{
            //    throw new ArgumentException("Invalid type. Argument must be of the type System.DateTime.");
            //}
            
            SetLocalizationCulture();

            String result;

            DateTime given = unixDateTime;

            DateTime current = DateTime.Now;

            TimeSpan difference = current - given;
            
            if (DateTimeFormatHelper.IsFutureDateTime(current, given))
            {
                // Future dates and times are not supported, but to prevent crashing an app
                // if the time they receive from a server is slightly ahead of the phone's clock
                // we'll just default to the minimum, which is "2 seconds ago".
                result = GetPluralTimeUnits(2, PluralSecondStrings);
            }

            if (difference.TotalSeconds > Year)
            {
                // "over a year ago"
                result = "一年多前";
            }
            else if (difference.TotalSeconds > (1.5 * Month))
            {
                // "x months ago"
                Int32 nMonths = (Int32)((difference.TotalSeconds + Month / 2) / Month);
                result = GetPluralMonth(nMonths);
            }
            else if (difference.TotalSeconds >= (3.5 * Week))
            {
                // "about a month ago"
                result = "约 1 个月前";
            }
            else if (difference.TotalSeconds >= Week)
            {
                Int32 nWeeks = (Int32)(difference.TotalSeconds / Week);
                if (nWeeks > 1)
                {
                    // "x weeks ago"
                    result = String.Format(CultureInfo.CurrentUICulture, "{0} 周前", nWeeks.ToString());
                }
                else
                {
                    // "about a week ago"
                    result = "约 1 周前";
                }
            }
            else if (difference.TotalSeconds >= (5 * Day))
            {
                // "last <dayofweek>"    
                result = GetLastDayOfWeek(given.DayOfWeek);
            }
            else if (difference.TotalSeconds >= Day)
            {
                // "on <dayofweek>"
                result = GetOnDayOfWeek(given.DayOfWeek);
            }
            else if (difference.TotalSeconds >= (2 * Hour))
            {
                // "x hours ago"
                Int32 nHours = (Int32)(difference.TotalSeconds / Hour);
                result = GetPluralTimeUnits(nHours, PluralHourStrings);
            }
            else if (difference.TotalSeconds >= Hour)
            {
                // "about an hour ago"
                result = "约 1 小时前";
            }
            else if (difference.TotalSeconds >= (2 * Minute))
            {
                // "x minutes ago"
                Int32 nMinutes = (Int32)(difference.TotalSeconds / Minute);
                result = GetPluralTimeUnits(nMinutes, PluralMinuteStrings);
            }
            else if (difference.TotalSeconds >= Minute)
            {
                // "about a minute ago"
                result = "约 1 分钟前";
            }
            else
            {
                // "x seconds ago" or default to "2 seconds ago" if less than two seconds.
                Int32 nSeconds = ((Int32)difference.TotalSeconds > 1.0) ? (Int32)difference.TotalSeconds : 2;
                result = GetPluralTimeUnits(nSeconds, PluralSecondStrings);
            }

            return result;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">(Not used).</param>
        /// <param name="targetType">(Not used).</param>
        /// <param name="parameter">(Not used).</param>
        /// <param name="culture">(Not used).</param>
        /// <returns>null</returns>
        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
