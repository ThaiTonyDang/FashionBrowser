using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionBrowser.Utilities
{
    public static class GlobalExtentions
    {
        public static string GetPriceFormat(this decimal price)
        {
            CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
            return string.Format(cultureInfo, "{0:C2}", price);
        }
        public static bool IsGuidParseFromString(this string str)
        {
            Guid guid;
            var result = Guid.TryParse(str, out guid);
            return result;
        }
    }
}
