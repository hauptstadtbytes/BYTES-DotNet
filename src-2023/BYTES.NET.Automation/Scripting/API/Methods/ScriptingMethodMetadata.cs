//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Automation.Scripting.API.Methods
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptingMethodMetadata: Attribute
    {
        #region public properties
        public string Name { get; set; }

        public string Description { get; set; }

        //public string[] Aliases { get; set; }

        #endregion

        #region public new instance method(s)

        public ScriptingMethodMetadata()
        {
            //this.Aliases = new string[] { };
        }

        #endregion
    }
}
