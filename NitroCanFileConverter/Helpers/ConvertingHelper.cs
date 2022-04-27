using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConverter.Helpers
{
    public static class ConvertingHelper
    {
        static bool isExtended(int number)
        { 
            return number > 0x7FF;
        }
        static int? HexStringToInt(string hexString)
        {
            if (int.TryParse(hexString, NumberStyles.HexNumber, null, out int Result))
            {
                return Result;
            }
            return null;
        }
        public static bool isExtended(string hexString)
        {
            var temp = HexStringToInt(hexString);
            if (temp is null) throw new ArgumentException();
            return isExtended((int)temp);
        }

    }


}
