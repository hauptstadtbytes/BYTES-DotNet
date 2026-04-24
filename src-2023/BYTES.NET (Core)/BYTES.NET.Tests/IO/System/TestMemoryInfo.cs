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
    internal class TestMemoryInfo
    {
        [TestMethod]
        public void TestMemoryCalculation()
        {
            MemoryInfo memory = new MemoryInfo((ulong)1024);

            Debug.WriteLine($"Memory in Bytes: {memory.InBytes}\n");
            Debug.WriteLine($"Memory in GB: {memory.InGB}");
            Assert.IsTrue(memory.InGB == 1);
        }
    }
}
