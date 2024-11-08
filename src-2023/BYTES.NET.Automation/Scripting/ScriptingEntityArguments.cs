//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) from BYTES.NET framework
using BYTES.NET.Collections;

namespace BYTES.NET.Automation.Scripting
{
    public class ScriptingEntityArguments : ExtendedDictionary<string, string>
    {
        #region protected properties

        protected override bool Serialize => false;

        protected override string XmlItemName => "Argument";
        protected override string XmlKeyName => "Name";
        protected override string XmlValueName => "Value";

        protected override bool EmbedStructure => false;

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        public ScriptingEntityArguments() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        #endregion
    }
}
