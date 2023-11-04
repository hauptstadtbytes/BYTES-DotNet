//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Primitives;

//import internal namespace(s) required
using BYTES.NET.Tests.Extensibility.API;

namespace BYTES.NET.Tests.Primitives
{
    [TestClass]
    public class TestTypeExtensions
    {
        [TestMethod]
        public void TestPropertyExists()
        {
            SampleMetadata data = new SampleMetadata() { Name = "Example" };

            Assert.AreEqual(true, data.GetType().PropertyExists("Name"));
            Assert.AreEqual(true, data.GetType().PropertyExists("Aliases"));
            Assert.AreEqual(false, data.GetType().PropertyExists("NotExisting"));
        }
    }
}
