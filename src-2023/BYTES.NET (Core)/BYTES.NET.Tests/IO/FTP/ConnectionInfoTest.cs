using BYTES.NET.IO;
using BYTES.NET.IO.FTP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Tests.IO.FTP
{
    [TestClass]
    public class ConnectionInfoTest
    {
        ConnectionInfo conn;

        [TestMethod]
        public void TestGetItems()
        {
            conn = new ConnectionInfo(
                "ftp://localhost:2121/",
                new UserInfo("testuser", "testpass"));

            FTPRemoteItem[] items = conn.GetItems();

            Assert.AreEqual(3, items.Length);

            Assert.IsTrue(items.Any(i => i.Name == "file1.txt"));
            Assert.IsTrue(items.Any(i => i.Name == "file2.txt"));
            Assert.IsTrue(items.Any(i => i.Name == "subdir"));
        }

    }
}
