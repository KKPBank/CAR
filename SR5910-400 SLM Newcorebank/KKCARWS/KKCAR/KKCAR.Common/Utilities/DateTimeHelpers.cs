using System;
using System.Globalization;

///<summary>
/// Class Name : DateTimeHelpers
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Common.Utilities
{
    public static class DateTimeHelpers
    {
        private static Nullable<DateTime> nullDateTime = new Nullable<DateTime>();

        #region "My Extensions"

        public static String FormatDateTime(this DateTime? date, string pattern)
        {
            string result = null;

            if (date.HasValue)
            {
                result = date.Value.ToString(pattern, CultureInfo.InvariantCulture);
            }

            return result;
        }

        public static String FormatDateTime(this DateTime date, string pattern)
        {
            return date.ToString(pattern, CultureInfo.InvariantCulture);
        }

        public static DateTime? ParseDateTime(this string strDate, string pattern)
        {
            DateTime date;

            bool no_error = DateTime.TryParseExact(strDate,
                                                   pattern,
                                                   CultureInfo.InvariantCulture,
                                                   DateTimeStyles.NoCurrentDateDefault,
                                                   out date);

            return no_error ? date : nullDateTime;
        }

        public static DateTime? ParseDateTime(this string strDate)
        {
            DateTime date;
            bool no_error = DateTime.TryParse(strDate, out date);
            return no_error ? date : nullDateTime;
        }

        public static String FormatTimestamp(this DateTime value)
        {
            return FormatDateTime(value, "yyMMddHHmmssff");
        }

        #endregion
    }
}