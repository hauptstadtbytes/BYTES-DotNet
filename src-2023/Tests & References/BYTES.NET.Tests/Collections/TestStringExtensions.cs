//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Collections;

namespace BYTES.NET.Tests.Collections
{
    [TestClass]
    public class TestStringExtensions
    {
        [TestMethod]
        public void TestOccurences()
        {
            List<string> myList = new List<string>() { "male", "male", "female", "male", "female", "male", "unknown", "male", "unknown" };
            Dictionary<string, int> occurences = myList.Occurences();

            Assert.AreEqual(3, occurences.Count);
            Assert.AreEqual(5, occurences["male"]);

            occurences = myList.ToArray().Occurences();

            Assert.AreEqual(3, occurences.Count);
            Assert.AreEqual(5, occurences["male"]);
        }
    }
}
