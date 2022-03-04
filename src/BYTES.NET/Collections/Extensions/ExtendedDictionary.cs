//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Collections;

namespace BYTES.NET.Collections.Extensions
{
    /// <summary>
    /// a set of extensions and helper method(s) for the 'ExtendedDictionary' class type
    /// </summary>
    public static class ExtendedDictionary
    {
        /// <summary>
        /// returns an array of missing keys
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string[] MissingKeys(this ExtendedDictionary<string,string> dictionary,string[] keys)
        {
            List<string> output = new List<string>();

            foreach (string key in keys)
            {
                if (!dictionary.ContainsKey(key))
                {
                    output.Add(key);
                }
            }

            return output.ToArray();
        }
    }
}
