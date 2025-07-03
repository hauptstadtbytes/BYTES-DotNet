using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public static class Formatter
    {

        public static double FormatMemory(ulong byteAmount, string displayUnit, bool fullUnitsOnly)
        {
            if (displayUnit.Equals("byte", StringComparison.OrdinalIgnoreCase) ||
                displayUnit.Equals("b", StringComparison.OrdinalIgnoreCase))
                return byteAmount;

            double divisor = displayUnit.ToLower() switch
            {
                "kb" => 1024,
                "mb" => 1024 * 1024,
                "gb" => 1024 * 1024 * 1024,
                "tb" => 1024L * 1024 * 1024 * 1024,
                _ => 1
            };

            double result = byteAmount / divisor;
            return fullUnitsOnly ? Math.Floor(result) : result;

        }
    }
}
