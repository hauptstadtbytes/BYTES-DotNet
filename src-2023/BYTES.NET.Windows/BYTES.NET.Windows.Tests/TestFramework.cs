//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BYTES.NET.Windows.Tests
{
    [TestClass]
    public class TestFramework
    {
        [TestMethod]
        public void TestFrameworkProperties()
        {
            //check for the 'bytes.net.windows.dll' assembly path
            Debug.WriteLine("The BYTES.NET.Windows library assembly is located at '" + Framework.AssemblyPath + "'");

            //check for the 'bytes.net.windows.dll' assembly directory path
            Debug.WriteLine("The BYTES.NET.Windows library assembly is located in folder '" + Framework.AssemblyDirectory + "'");
        }
    }
}
