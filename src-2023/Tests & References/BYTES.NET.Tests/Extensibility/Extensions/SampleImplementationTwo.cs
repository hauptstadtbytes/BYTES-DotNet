//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Tests.Extensibility.API;

namespace BYTES.NET.Tests.Extensibility.Extensions
{
    [SampleMetadata(Name = "TestImplementationTwo", Aliases = new string[] { "TestImplementationTwo" })]
    public class SampleImplementationTwo : ISampleInterface
    {
        public string Transform(string text)
        {
            return "Hello World!";
        }
    }
}
