﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Test.IO.Extensibility.API;

namespace BYTES.NET.Test.IO.Extensibility.Implementations
{
    [TestMetadata(Name = "TestImplementationThree", Aliases = new string[] { "TestImplementationThree" })]
    public class TestImplementationThree : ITestInterface
    {
        public string Transform(string text)
        {
            return text.Length.ToString();
        }
    }
}
