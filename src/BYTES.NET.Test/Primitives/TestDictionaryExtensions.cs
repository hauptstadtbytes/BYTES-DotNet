//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Collections.Generic;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Primitives.Extensions;

namespace BYTES.NET.Test.Primitives
{
    [TestClass]
    public class TestDictionaryExtensions
    {
        [TestMethod]
        public void TestAggregations()
        {
            Dictionary<string, string> names = new Dictionary<string, string>() { { "Peter", "Peter" }, { "Paul", "Paul" }, { "Mary", "Mary" } };
            Dictionary<string, List<string>> namesAggregated = names.Aggregated(new Dictionary<string, string>() { { "Peter", "Male" }, { "Paul", "Male" }, { "Mary", "Female" } });

            Assert.AreEqual(2, namesAggregated.Count);
            Assert.AreEqual(2, namesAggregated["Male"].Count);
            Assert.AreEqual("Mary", namesAggregated["Female"][0]);

            Dictionary<string, int> namesCounts = new Dictionary<string, int>() { { "Peter", 3 }, { "Paul", 2 }, { "Mary", 4 }, { "Unknown", 42 } };
            Dictionary<string, int> namesCountsAggregated = namesCounts.Aggregated(new Dictionary<string, string>() { { "Peter", "Male" }, { "Paul", "Male" }, { "Mary", "Female" } });

            Assert.AreEqual(3, namesCountsAggregated.Count);

            foreach(KeyValuePair<string, int> kvp in namesCountsAggregated)
            {
                Debug.WriteLine(kvp.Key + " " + kvp.Value);
            }

            Assert.AreEqual(5, namesCountsAggregated["Male"]);
            Assert.AreEqual(4, namesCountsAggregated["Female"]);
            Assert.AreEqual(42, namesCountsAggregated["Unknown"]);
        }
    }
}
