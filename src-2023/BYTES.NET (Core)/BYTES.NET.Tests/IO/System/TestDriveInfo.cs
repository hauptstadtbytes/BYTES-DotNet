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
    public class TestDriveInfo
    {
        public BYTES.NET.IO.System.DriveInfo drive;

        [TestInitialize]
        public void TestSetup()
        {
            drive = new NET.IO.System.DriveInfo("C");
        }

        [TestMethod]
        public void TestProperties()
        {
            Console.WriteLine($"Drive Type: {drive.Type}");

            Assert.IsNotNull(drive.Type);

            Console.WriteLine($"Driver ready: {drive.IsReady}");

            Assert.AreNotEqual(false, drive.IsReady);

            Console.Write($"Is removable: {drive.IsRemovable}");

            Assert.AreNotEqual(drive.IsRemovable, true);

            Console.Write($"Path: {drive.Path}");

            Assert.IsNotNull(drive.Path);

            MemoryInfo totalSpace = drive.TotalSpace();

            Console.WriteLine($"Total Space: {totalSpace}");

            Assert.AreNotEqual(0, totalSpace.InBytes);

            MemoryInfo freeSpace = drive.FreeSpace();

            Console.WriteLine($"Free Space: {freeSpace}");

            Assert.AreNotEqual(0, freeSpace.InBytes);
        }
    }
}
