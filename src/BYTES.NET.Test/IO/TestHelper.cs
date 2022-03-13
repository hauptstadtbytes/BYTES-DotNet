//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

using System.Collections.Generic;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO;

namespace BYTES.NET.Test.IO
{
    [TestClass]
    public class TestHelper
    {
        [TestMethod]
        public void TestPathMethods()
        {
            //check for the 'bytes.net.dll' assembly path
            Trace.WriteLine("The BYTES.NET library assembly is located at '" + Helper.GetLibraryAssemblyPath() + "'");

            //check for the 'bytes.net.dll' assembly directory path
            Trace.WriteLine("The BYTES.NET library assembly is located in folder '" + Helper.GetLibraryDirPath() + "'");

            //test expanding (Windows default) variable(s) in (file system) path
            string inputPath = "%ProgramFiles%";
            string result = Helper.ExpandPath(inputPath);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + result + "'");

            //test expanding custom variable(s) in (file system) path
            inputPath = "%bytes.net%";

            result = Helper.ExpandPath(inputPath);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + result + "'");
            Assert.AreEqual(Helper.GetLibraryAssemblyPath(), result);

            result = Helper.ExpandPath(inputPath, null, false);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + result + "'");
            Assert.AreEqual("%bytes.net%", result);

            inputPath = @"%bytes.net.dir%";
            result = Helper.ExpandPath(inputPath, null);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + result + "'");
            Assert.AreEqual(Helper.GetLibraryDirPath(), result);

            //check for URLs in the path
            inputPath = "https://eux.jacando.io/x/api";
            result = Helper.ExpandPath(inputPath, null);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + result + "'");
            Assert.AreEqual(inputPath, result);

            //test expanding wildcard path(s)
            inputPath = "%bytes.net.dir%";
            string[] resultArray = Helper.ExpandWildcardPath(inputPath);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(Helper.GetLibraryDirPath().ToString(), resultArray[0]);

            inputPath = "%bytes.net.dir%\\..\\..\\..\\..\\..\\test\\IOObjects\\DummyFolder\\*.dll";
            resultArray = Helper.ExpandWildcardPath(inputPath);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(2, resultArray.Length);

            inputPath = "%bytes.net.dir%\\..\\..\\..\\..\\..\\testFailes\\IOObjects\\DummyFolder\\*.dll"; //test handling a non-existing path
            resultArray = Helper.ExpandWildcardPath(inputPath);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(0, resultArray.Length);

            //inputPath = "%bytes.net.dir%\\..\\..\\..\\..\\..\\samples\\IOObjects\\*Folder\\";
            inputPath = "%bytes.net.dir%\\..\\..\\..\\..\\..\\test\\IOObjects\\D*Folder\\";
            resultArray = Helper.ExpandWildcardPath(inputPath);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(2, resultArray.Length);

            inputPath = "%bytes.net.dir%\\..\\..\\..\\..\\..\\test\\IOObjects\\D*Folder";
            resultArray = Helper.ExpandWildcardPath(inputPath);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(2, resultArray.Length);

            inputPath = "%bytes.net.dir%\\..\\..\\..\\..\\..\\test\\IOObjects\\D*Folder\\Sub*";
            resultArray = Helper.ExpandWildcardPath(inputPath);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(2, resultArray.Length);

            inputPath = "%bytes.net.dir%\\..\\..\\..\\..\\..\\test\\IOObjects\\D*Folder\\Sub*\\*.txt";
            resultArray = Helper.ExpandWildcardPath(inputPath);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(1, resultArray.Length);

            Dictionary<string, string> variables = new Dictionary<string, string>() { { "demo.variable", Helper.ExpandPath("%bytes.net.dir%") } };

            inputPath = "%demo.variable%\\..\\..\\..\\..\\..\\test\\IOObjects\\D*Folder\\Sub*\\*.dll";
            resultArray = Helper.ExpandWildcardPath(inputPath, variables);
            Trace.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(1, resultArray.Length);

            string[] inputArray = new string[]
            {
                "%bytes.net.dir%\\..\\..\\..\\..\\..\\test\\IOObjects\\D*Folder\\Sub*\\*.dll",
                "%demo.variable%\\..\\..\\..\\..\\..\\test\\IOObjects\\D*Folder\\Sub*\\*.dll"
            };
            resultArray = Helper.ExpandWildcardPath(inputArray, variables);
            Trace.WriteLine("Expanding variable(s) in '" + System.String.Join(",", inputArray) + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(1, resultArray.Length);
        }
    }
}
