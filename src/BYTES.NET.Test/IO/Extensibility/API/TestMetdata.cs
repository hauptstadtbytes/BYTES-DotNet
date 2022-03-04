using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Test.IO.Extensibility.API
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TestMetadata : Attribute
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string[] Aliases { get; set; }

        public TestMetadata()
        {
            this.Aliases = new string[] { };
        }
    }
}
