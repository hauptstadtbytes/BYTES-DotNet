//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// converts a Dictionary of strings to a 'NameValueCollection'
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks><seealso href="https://social.msdn.microsoft.com/Forums/vstudio/en-US/720c4bc5-d449-4a19-b9d2-f35c2bbc5795/what-is-the-difference-between-namevalue-collection-and-dictionarylttgt?forum=netfxbcl" /> and <seealso href="https://code-examples.net/en/q/6e53af"/></remarks>
        public static NameValueCollection ToNameValueCollection(this Dictionary<string,string> source)
        {
            NameValueCollection output = new NameValueCollection();

            foreach (KeyValuePair<string, string> pair in source)
            {
                output.Add(pair.Key, pair.Value);
            }

            return output;
        }

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
