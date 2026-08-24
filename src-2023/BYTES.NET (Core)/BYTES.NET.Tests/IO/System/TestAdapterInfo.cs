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
    public class TestAdapterInfo
    {
        private AdapterInfo adapter;

        [TestInitialize]
        public void Setup()
        {
            NetworkInterface intrfc = NetworkInterface.GetAllNetworkInterfaces().First();

            adapter = new AdapterInfo(intrfc);
        }

        [TestMethod]
        public void TestProperties()
        {
            Console.WriteLine($"Adapter Name: {adapter.Name}");

            Assert.IsNotNull(adapter.Name);

            Console.WriteLine($"Adapter Description: {adapter.Description}");

            Assert.IsNotNull(adapter.Description);

            Console.WriteLine($"Adapter ID: {adapter.Id}");

            Assert.IsNotNull(adapter.Id);

            Console.WriteLine($"Adapter Address: {adapter.Address}");

            Assert.IsNotNull(adapter.Address);

        }
    }
}
