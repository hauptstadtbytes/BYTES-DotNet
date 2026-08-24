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
            //Test drive C
            drive = new NET.IO.System.DriveInfo("C");
        }

        [TestMethod]
        public void TestProperties()
        {
            //Test Type 
            Console.WriteLine($"Drive Type: {drive.Type}");

            Assert.IsNotNull(drive.Type);

            //Test IsReady
            Console.WriteLine($"Driver ready: {drive.IsReady}");

            Assert.AreNotEqual(false, drive.IsReady);

            //Test IsRemovable
            Console.Write($"Is removable: {drive.IsRemovable}");

            Assert.AreNotEqual(drive.IsRemovable, true);

            //Test Path
            Console.Write($"Path: {drive.Path}");

            Assert.IsNotNull(drive.Path);

            //Test Total Space
            MemoryInfo totalSpace = drive.TotalSpace();

            Console.WriteLine($"Total Space: {totalSpace}");

            Assert.AreNotEqual(0, totalSpace.InBytes);

            //Test Free Space
            MemoryInfo freeSpace = drive.FreeSpace();

            Console.WriteLine($"Free Space: {freeSpace}");

            Assert.AreNotEqual(0, freeSpace.InBytes);
        }
    }
}
