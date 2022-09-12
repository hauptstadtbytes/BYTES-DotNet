//import internal namespace(s) required
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'Newtonsoft.json' framework
using Newtonsoft.Json.Linq;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.AOP;

namespace BYTES.NET.Docu.App.NETFULL.Types.AOP.JSON
{
    public class LazyJSONObject : MarshalByRefObject
    {
        #region protected properties

        protected JObject _raw;

        #endregion

        #region public properties

        public JObject Raw => _raw;

        #endregion

        #region public new instance method(s)

        public LazyJSONObject(JObject raw)
        {
            _raw = raw;
        }

        #endregion

        #region public static method(s)

        public static T Create<T>(JObject raw, AOPAction<T>[] actions = null) where T : LazyJSONObject
        {
            //create the data instance
            T item = (T)Activator.CreateInstance(typeof(T), raw);

            //return the output value
            return AOPProxy<T>.Create(item, actions) as T;
        }

        public static T Create<T>(string rawString, AOPAction<T>[] actions = null) where T : LazyJSONObject
        {
            //parse the string data
            JObject raw = JObject.Parse(rawString);

            //create the data instance
            T item = (T)Activator.CreateInstance(typeof(T), raw);

            //return the output value
            return AOPProxy<T>.Create(item, actions) as T;
        }

        #endregion
    }
}
