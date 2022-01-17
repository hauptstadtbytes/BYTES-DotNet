//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Metadata : Attribute
    {
        public string Name { get; set; }

        public string[] Aliases { get; set; }

        public Metadata()
        {
            this.Aliases = new string[] { };
        }
    }
}
