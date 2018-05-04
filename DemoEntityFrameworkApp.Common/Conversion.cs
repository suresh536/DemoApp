using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEntityFrameworkApp.Common
{
    public static class Conversion
    {
        public static int ToInt16(this object value)
        {
            if(value==DBNull.Value)
            {
                return 0;
            }
            else
            {
                return (Convert.ToInt16(value,CultureInfo.CurrentCulture.NumberFormat));
            }
        }

        public static int ToInt16(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
            {
                return (Convert.ToInt16(value, CultureInfo.CurrentCulture.NumberFormat));
            }
        }

        public static int ToInt32(this object value)
        {
            if (value == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return (Convert.ToInt32(value, CultureInfo.CurrentCulture.NumberFormat));
            }
        }

        public static int ToInt32(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            else
            {
                return (Convert.ToInt32(value, CultureInfo.CurrentCulture.NumberFormat));
            }
        }

        public static string ToStr(this object value)
        {
            return ToString(value);
        }

        public static string ToString(this object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return "";
            }
            else
            {
                return (Convert.ToString(value, CultureInfo.CurrentCulture).Trim());
            }

        }

        public static Decimal ToDecimal(this object value)
        {
            if (value == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return (Convert.ToDecimal(value, CultureInfo.CurrentCulture.NumberFormat));
            }
        }
    }
}
