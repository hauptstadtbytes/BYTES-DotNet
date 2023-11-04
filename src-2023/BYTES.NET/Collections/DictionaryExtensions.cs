//import (default) .NET namespace(s) required 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Collections
{
    /// <summary>
    /// 'Dictionary' type extensions
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// aggregates 'Dictionary'-stored items by key categories
        /// </summary>
        /// <param name="source"></param>
        /// <param name="categories">the 'key' defines the source string and the 'value' the target category</param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> AggregatedByKeyCategories(this Dictionary<string, string> source, Dictionary<string, string> categories)
        {
            //categprize and aggregate the value(s)
            Dictionary<string, List<string>> output = new Dictionary<string, List<string>>();

            foreach (KeyValuePair<string, string> pair in source)
            {
                string key = pair.Key;

                if (categories.ContainsKey(pair.Key))
                {
                    key = categories[pair.Key];
                }

                if (!output.ContainsKey(key))
                {
                    output.Add(key, new List<string>());
                }

                output[key].Add(pair.Value);
            }

            //return the output value
            return output;
        }

    }
}
