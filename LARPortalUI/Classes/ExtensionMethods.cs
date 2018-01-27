using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace LarpPortal.Classes
{
    [Serializable()]
    public static class ExtensionsMethods
    {
        /// <summary>
        /// Get substring of specified number of characters on the right.
        /// </summary>
        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }

        public static void ForAll<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
                action(item);
        }

        public static string AddCarriageReturn(this string value)
        {

            return value + Environment.NewLine;
        }

        public static Int32 ToInt32(this string value)
        {
            return Convert.ToInt32(value.Trim());
        }

        public static Boolean ToBoolean(this string value)
        {
            return Convert.ToBoolean(value.Trim());
        }

        public static string ToUIString(this string value)
        {
            return value.ToString(CultureInfo.CurrentUICulture);
        }
    }
}
