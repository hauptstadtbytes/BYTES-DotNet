//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

using System.Collections.Generic;

//import .net namespace(s) required
using BYTES.NET.Primitives.Extensions;

namespace BYTES.NET.Test.Primitives
{
    [TestClass]
    public class TestListExtensions
    {
        [TestMethod]
        public void TestStringAggregation()
        {
            List<string> myList = new List<string>() { "male", "male", "female", "male", "female", "male", "unknown", "male", "unknown"};
            Dictionary<string, int> aggregations = myList.Aggregated();

            Assert.AreEqual(3, aggregations.Count);
            Assert.AreEqual(5, aggregations["male"]);
        }
    }
}
