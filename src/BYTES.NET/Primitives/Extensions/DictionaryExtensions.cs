//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// aggregates a dictionary holding 'string'-'string'-pairs
        /// </summary>
        /// <param name="source"></param>
        /// <param name="aggregations"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> Aggregated(this Dictionary<string,string> source, Dictionary<string, string>? aggregations = null)
        {
            //parse the argument(s)
            if (aggregations == null)
            {
                aggregations = new Dictionary<string, string>();
            }

            //aggregate the value(s)
            Dictionary<string, List<string>> output = new Dictionary<string, List<string>>();

            foreach(KeyValuePair<string, string> pair in source)
            {
                string key = pair.Key;

                if (aggregations.ContainsKey(pair.Key))
                {
                    key = aggregations[pair.Key];
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

        /// <summary>
        /// aggregates a dictionary holding 'string'-'integer'-pairs
        /// </summary>
        /// <param name="source"></param>
        /// <param name="aggregations"></param>
        /// <returns></returns>
        public static Dictionary<string, int> Aggregated(this Dictionary<string, int> source, Dictionary<string, string>? aggregations = null)
        {
            //parse the argument(s)
            if (aggregations == null)
            {
                aggregations = new Dictionary<string, string>();
            }

            //aggregate the value(s)
            Dictionary<string, int> output = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> pair in source)
            {
                string key = pair.Key;

                if (aggregations.ContainsKey(pair.Key))
                {
                    key = aggregations[pair.Key];
                }

                if (!output.ContainsKey(key))
                {
                    output.Add(key, 0);
                }

                output[key] += pair.Value;
            }

            //return the output value
            return output;
        }
    }
}
