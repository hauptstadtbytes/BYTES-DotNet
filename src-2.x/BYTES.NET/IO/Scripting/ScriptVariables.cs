//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Collections;

namespace BYTES.NET.IO.Scripting
{
    public class ScriptVariables : ExtendedDictionary<string,object>
    {
        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <remarks>makes the dictionary case insensitive</remarks>
        public ScriptVariables() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// gets a dictionary of the 'string' type reperesentations of all variables
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> Get()
        {
            Dictionary<string,string> output = new Dictionary<string, string>();

            foreach(KeyValuePair<string,object> pair in this)
            {
                output.Add(pair.Key, pair.Value.ToString());
            }

            return output;
        }

        /// <summary>
        /// gets a variable value (by it's name)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Get<T>(string name)
        {
            if (this.ContainsKey(name))
            {
                return (T)this[name];
            }

            return default(T);
        }

        /// <summary>
        /// set a variable value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Set(string name, object value)
        {
            if (!this.ContainsKey(name))
            {
                this.Add(name, value);
            }

            this[name] = value;
        }

        #endregion
    }
}
