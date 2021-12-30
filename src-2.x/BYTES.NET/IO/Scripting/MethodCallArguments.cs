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
    [Serializable]
    public class MethodCallArguments : ExtendedDictionary<string,string>
    {

        #region protected properties

        protected override bool Serialize => false;

        protected override string XmlItemName => "Parameter";
        protected override string XmlKeyName => "Name";

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <remarks>makes the dictionary case insensitive</remarks>
        public MethodCallArguments() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        #endregion

    }
}
