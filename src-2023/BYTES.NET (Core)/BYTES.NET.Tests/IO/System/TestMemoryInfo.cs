using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using BYTES.NET.IO.System;

namespace BYTES.NET.Tests.IO.System
{

    [TestClass]
    public class TestMemoryInfo
    {
        private MemoryInfo memory;

        [TestInitialize]
        public void Setup()
        {
            memory = new MemoryInfo((ulong)1024);
        }


        //Test if the memory is converted correctly
        [TestMethod]
        public void TestMemoryCalculation()
        {
            Debug.WriteLine($"Memory in Bytes: {memory.InBytes}");

            Debug.WriteLine($"Memory in MB: {memory.InKB}");

            Assert.IsTrue(memory.InKB == 1);
        }
    }
}
