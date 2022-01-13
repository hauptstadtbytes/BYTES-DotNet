//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Metdata : Attribute
    {
        public string Name { get; set; }

        public string[] Aliases { get; set; }

        public Metdata()
        {
            this.Aliases = new string[] { };
        }
    }
}
