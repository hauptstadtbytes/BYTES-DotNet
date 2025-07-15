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
    public class TestInfo
    {
        private Info info;

        [TestInitialize]
        public void Setup()
        {
            info = new Info();
        }

        [TestMethod]
        public void TestName()
        {
            string machineName = Environment.MachineName;
            Debug.WriteLine($"System Name: {info.Name}");
            Assert.AreEqual(machineName, info.Name);
        }

        [TestMethod]
        public void TestMemory()
        {
            double memoryInGB = info.Memory();
            Debug.WriteLine($"Memory (GB): {memoryInGB}");
            Assert.IsTrue(memoryInGB > 0, "Memory should be greater than 0 GB.");
        }

        [TestMethod]
        public void TestProcessors()
        {
            int processorCount = Environment.ProcessorCount;
            Debug.WriteLine($"Processors: {info.Processors}");
            Assert.AreEqual(processorCount, info.Processors);
        }

        [TestMethod]
        public void TestAdapters()
        {
            var adapters = info.Adapters;
            Debug.WriteLine($"Adapter Types Count: {adapters.Count}");
            Assert.IsTrue(adapters.Count > 0);

            foreach (var kvp in adapters)
            {
                Debug.WriteLine($"Adapter Type: {kvp.Key}, Count: {kvp.Value.Count}");
                Assert.IsNotNull(kvp.Value);
            }
        }

        [TestMethod]
        public void TestGetAdaptersByType()
        {
            var ethernetAdapters = info.GetAdapters(NetworkInterfaceType.Ethernet);
            Debug.WriteLine($"Ethernet Adapters Count: {ethernetAdapters.Length}");
            Assert.IsNotNull(ethernetAdapters);
        }

        [TestMethod]
        public void TestCurrentUser()
        {
            var user = info.CurrentUser;
            Debug.WriteLine($"Current User: {user.Name}, Domain: {user.Domain}");
            Assert.AreEqual(Environment.UserName, user.Name);
            Assert.AreEqual(Environment.UserDomainName, user.Domain);
        }

        [TestMethod]
        public void TestDrives()
        {
            var drives = info.Drives;
            Debug.WriteLine($"Drive Types Count: {drives.Count}");
            Assert.IsTrue(drives.Count > 0);

            foreach (var kvp in drives)
            {
                Debug.WriteLine($"Drive Type: {kvp.Key}, Count: {kvp.Value.Count}");
                Assert.IsNotNull(kvp.Value);
            }
        }

        [TestMethod]
        public void TestGetDrivesByType()
        {
            var fixedDrives = info.GetDrives(DriveType.Fixed);
            Debug.WriteLine($"Fixed Drives Count: {fixedDrives.Length}");
            Assert.IsNotNull(fixedDrives);
        }
    }
}
