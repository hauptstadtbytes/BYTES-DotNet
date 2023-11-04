//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Collections
{
    /// <summary>
    /// string type extension method(s)
    /// </summary>
    public static class StringExtensions
    {
        #region public method(s)

        /// <summary>
        /// counts the occurences of strings inside a string array given
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Dictionary<string, int> Occurences(this string[] list)
        {
            //create the output value
            Dictionary<string, int> output = new Dictionary<string, int>();

            foreach (string item in list)
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

        /// <summary>
        /// counts the value occurences inside a list of strings
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Dictionary<string, int> Occurences(this List<string> list)
        {
            return list.ToArray().Occurences();
        }

        #endregion
    }
}
