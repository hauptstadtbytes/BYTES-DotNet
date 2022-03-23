//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

//import .net namespace(s) required
using BYTES.NET.Primitives.Extensions;

namespace BYTES.NET.Test.Primitives
{
    [TestClass]
    public class TestObjectExtensions
    {
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
                                                                                            if (item.GetType() == typeof(string)) {
                                                                                                return item.ToString().Length;
                                                                                            } else
                                                                                            {
                                                                                                return default;
                                                                                            }
                                                                                        }));
            Assert.AreEqual(5, intOutput);
        }

    }
}
