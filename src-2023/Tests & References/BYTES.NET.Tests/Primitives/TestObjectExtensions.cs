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
    public class TestObjectExtensions
    {
        [TestMethod]
        public void TestPropertyIsNullOrDefault()
        {
            SampleMetadata data = new SampleMetadata() { Name = "Example" };

            Assert.AreEqual(false, data.PropertyIsNullOrDefault("Name"));
            Assert.AreEqual(true, data.PropertyIsNullOrDefault("Description"));
        }
            
        [TestMethod]
        public void TestConversions()
        {
            //test string > bool conversions
            object? input = "true";

            bool boolOutput;
            Assert.AreEqual(true, input.TryConvert<bool>(out boolOutput));
            Assert.AreEqual(true, boolOutput);

            input = "Dummy";
            Assert.AreEqual(false, input.TryConvert<bool>(out boolOutput));

            //test string > integer conversions
            input = "12";

            int intOutput;
            Assert.AreEqual(true, input.TryConvert<int>(out intOutput));
            Assert.AreEqual(12, intOutput);

            input = "Dummy";
            Assert.AreEqual(false, input.TryConvert<int>(out intOutput));

            //test custom conversions
            Assert.AreEqual(true, input.TryConvert<int>(out intOutput, (object item) => {
                if (item.GetType() == typeof(string))
                {
                    return item.ToString().Length;
                }
                else
                {
                    return default;
                }
            }));
            Assert.AreEqual(5, intOutput);
        }
    }
}
