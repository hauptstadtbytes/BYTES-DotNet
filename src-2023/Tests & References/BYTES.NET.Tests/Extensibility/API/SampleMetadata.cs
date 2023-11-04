//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Tests.Extensibility.API
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SampleMetadata : Attribute
    {
        #region public properties
        public string Name { get; set; }

        public string Description { get; set; }

        public string[] Aliases { get; set; }

        #endregion

        #region public new instance method(s)

        public SampleMetadata()
        {
            this.Aliases = new string[] { };
        }

        #endregion
    }
}
