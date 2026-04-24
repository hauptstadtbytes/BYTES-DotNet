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
    public class TestSystemInfo
    {
        private SystemInfo info;

        [TestInitialize]
        public void Setup()
        {
            info = new SystemInfo();
        }

        [TestMethod]
        public void TestProperties()
        {
            // Test Name
            string machineName = Environment.MachineName;
            Debug.WriteLine($"System Name: {info.Name}");
            Assert.AreEqual(machineName, info.Name);

            //Test Memory (RAM)
            MemoryInfo memory = info.Memory();
            Debug.WriteLine($"Memory (in GB): {memory.InGB}");
            Assert.IsTrue(memory.InBytes > 0, "Memory should be greater than 0");

            //Test Processors
            int processorCount = Environment.ProcessorCount;
            Debug.WriteLine($"Processors: {info.Processors}");
            Assert.AreEqual(processorCount, info.Processors);

            //Test Adapters
            var adapters = info.Adapters;
            Debug.WriteLine($"Adapter Types Count: {adapters.Count}");
            Assert.IsTrue(adapters.Count > 0);

            foreach (var kvp in adapters)
            {
                Debug.WriteLine($"Adapter Type: {kvp.Key}, Count: {kvp.Value.Count}");
                Assert.IsNotNull(kvp.Value);
            }

            //Test User and domain
            var user = info.User;
            Debug.WriteLine($"Current User: {user.Name}, Domain: {user.Domain}");
            Assert.AreEqual(Environment.UserName, user.Name);
            Assert.AreEqual(Environment.UserDomainName, user.Domain);

            //Test Drives
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
        public void TestGetAdapters()
        {
            var ethernetAdapters = info.GetAdapters(NetworkInterfaceType.Ethernet);
            Debug.WriteLine($"Ethernet Adapters Count: {ethernetAdapters.Length}");
            Assert.IsNotNull(ethernetAdapters);
        }

        [TestMethod]
        public void TestGetDrives()
        {
            var fixedDrives = info.GetDrives(DriveType.Fixed);
            Debug.WriteLine($"Fixed Drives Count: {fixedDrives.Length}");
            Assert.IsNotNull(fixedDrives);
        }

        [TestMethod]
        public void TestFixedDriveSizes()
        {
            var fixedDrives = info.GetDrives(DriveType.Fixed);

            Assert.IsNotNull(fixedDrives);

            foreach (var drive in fixedDrives)
            {
                var totalGB = drive.TotalSpace().InGB;
                Debug.WriteLine($"Fixed Drive: {drive.Path}, Size: {totalGB} GB");
                Assert.IsTrue(totalGB > 0);
            }
        }
    }
}