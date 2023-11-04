//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BYTES.NET.Tests
{
    [TestClass]
    public class TestFramework
    {
        [TestMethod]
        public void TestFrameworkProperties()
        {
            //check for the 'bytes.net.dll' assembly path
            Debug.WriteLine("The BYTES.NET library assembly is located at '" + Framework.AssemblyPath + "'");

            //check for the 'bytes.net.dll' assembly directory path
            Debug.WriteLine("The BYTES.NET library assembly is located in folder '" + Framework.AssemblyDirectory + "'");
        }
    }
}
