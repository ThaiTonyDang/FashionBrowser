using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Utilities
{
    public static class GlobalExtentions
    {
        public static string GetPriceFormat(this decimal price)
        {
            var cultureInfo = CultureInfo.GetCultureInfo("vi-VN");
            return price.ToString("#,### VND" , cultureInfo.NumberFormat);
        }
        public static bool IsGuidParseFromString(this string str)
        {
            Guid guid;
            var result = Guid.TryParse(str, out guid);
            return result;
        }

        public static bool IsParseDateTime(this string date)
        {
            DateTime dateTime;
            var result = DateTime.TryParse(date, out dateTime);
            return result;
        }
    }
}
