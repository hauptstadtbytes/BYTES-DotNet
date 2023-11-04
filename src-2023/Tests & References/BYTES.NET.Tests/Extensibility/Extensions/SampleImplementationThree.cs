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
    [SampleMetadata(Name = "TestImplementationThree", Aliases = new string[] { "TestImplementationThree" })]
    public class SampleImplementationThree : ISampleInterface
    {
        public string Transform(string text)
        {
            return text.Length.ToString();
        }
    }
}
