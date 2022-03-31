//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives.Extensions
{
    public static class ListExtensions
    {
        public static Dictionary<string,int> Aggregated(this List<string> sorce)
        { 
            //create the output value
            Dictionary<string,int> output = new Dictionary<string,int>();

            foreach(string item in sorce)
            {
                if (!output.ContainsKey(item))
                {
                    output.Add(item, 0);
                }

                output[item]++;
            }

            //return the output value
            return output;
        }
    }
}
