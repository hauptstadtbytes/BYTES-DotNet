//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Collections;
using System.Diagnostics;

namespace BYTES.NET.Tests.Collections
{
    [TestClass]
    public class TestIndexingList
    {
        [TestMethod]
        public void TestIndexing()
        {
            IndexingList<string, int> list = new IndexingList<string, int>(GetIndex, ValidateIndex) { "Hello", "World", "My", "Age", "Is", "42"};

            foreach(int index in list.Indices)
            {
                Debug.WriteLine(index);
            }

            Assert.AreEqual(3, list.Indices.Length);
            Assert.AreEqual(1, list[3].Length);
            Assert.AreEqual("Hello", list[5][0]);

        }

        private int GetIndex(string value)
        {
            return value.Length;
        }

        private bool ValidateIndex(string value, int index)
        {
            if(value.Length == index)
            {
                return true;
            }

            return false;
        }

    }
}
