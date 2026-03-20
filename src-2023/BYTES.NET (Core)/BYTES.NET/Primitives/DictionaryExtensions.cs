//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives
{
    /// <summary>
    /// 'Dicrionary' type extension method(s)
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// converts a Dictionary of strings to a 'NameValueCollection'
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks><seealso href="https://social.msdn.microsoft.com/Forums/vstudio/en-US/720c4bc5-d449-4a19-b9d2-f35c2bbc5795/what-is-the-difference-between-namevalue-collection-and-dictionarylttgt?forum=netfxbcl" /> and <seealso href="https://code-examples.net/en/q/6e53af"/></remarks>
        public static NameValueCollection ToNameValueCollection(this Dictionary<string, string> source)
        {
            NameValueCollection output = new NameValueCollection();

            foreach (KeyValuePair<string, string> pair in source)
            {
                output.Add(pair.Key, pair.Value);
            }

            return output;
        }
    }
}
