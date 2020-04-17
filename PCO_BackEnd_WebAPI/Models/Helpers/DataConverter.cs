using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Helpers
{
    public static class DataConverter
    {
        public static bool ConvertToDateTime(string value, out DateTime dateTime)
        {
            if (DateTime.TryParse(value, new CultureInfo("fil-PH"), DateTimeStyles.None, out dateTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ConvertToInt(string value, out int result)
        {
            if (Int32.TryParse(value, out result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}