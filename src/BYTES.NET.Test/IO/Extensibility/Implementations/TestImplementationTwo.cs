using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Test.IO.Extensibility.API;

namespace BYTES.NET.Test.IO.Extensibility.Implementations
{
    [TestMetadata(Name = "TestImplementationTwo", Aliases = new string[] { "TestImplementationTwo" })]
    public class TestImplementationTwo : ITestInterface
    {
        public string Transform(string text)
        {
            return "Hello World!";
        }
    }
}
