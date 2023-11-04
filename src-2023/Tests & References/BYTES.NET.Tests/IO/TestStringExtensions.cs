//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO;

namespace BYTES.NET.Tests.IO
{
    [TestClass]
    public class TestStringExtensions
    {

        [TestMethod]
        public void TestCheckForFilePath()
        {
            string theText = "%BYTES.NET.DIR%\\..\\..\\..\\..\\test\\CLI_SampleSettings.xml\\";
            Assert.AreEqual(false, theText.CheckForFilePath());

            theText = "%BYTES.NET.DIR%\\..\\..\\..\\..\\test\\CLI_SampleSettings.xml";
            Assert.AreEqual(true, theText.CheckForFilePath());

            theText = "D:\\MightBeAFile.docx";
            Assert.AreEqual(true, theText.CheckForFilePath());

            theText = "D:\\MightBeAFolder";
            Assert.AreEqual(false, theText.CheckForFilePath());

            //check for URL behavior
            string inputPath = "https://mockup.bytesapp.de/RESTAPI/echo";
            string result = inputPath.ExpandPath();
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + result + "'");
            Assert.AreEqual(inputPath, result);
        }

        [TestMethod]
        public void TestCheckForURL()
        {
            string inputPath = "https://mockup.bytesapp.de/RESTAPI/echo";
            Assert.AreEqual(true, inputPath.CheckForURL());
        }

        [TestMethod]
        public void TestExpanding()
        {
            //test expanding (Windows default) variable(s) in (file system) path
            string inputPath = "%ProgramFiles%";
            string result = inputPath.ExpandPath();
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + result + "'");

            //test expanding custom variable(s) in (file system) path
            inputPath = "%bytes.net%";

            result = inputPath.ExpandPath();
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + result + "'");
            Assert.AreEqual(Framework.AssemblyPath, result);

            result = inputPath.ExpandPath(null, false);
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + result + "'");
            Assert.AreEqual("%bytes.net%", result);

            inputPath = @"%bytes.net.dir%";
            result = inputPath.ExpandPath(null);
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + result + "'");
            Assert.AreEqual(Framework.AssemblyDirectory, result);
        }

        [TestMethod]
        public void TestWildcardsExpanding()
        {
            string inputPath = "%bytes.net.dir%";
            string[] resultArray = inputPath.ExpandWildcardPath();
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(Framework.AssemblyDirectory, resultArray[0]);

            inputPath = "%bytes.net.dir%\\..\\..\\Debug\\net472\\*.dll";
            resultArray = inputPath.ExpandWildcardPath();
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(10, resultArray.Length);

            inputPath = "%bytes.net.dir%\\..\\..\\Debug\\net472-NotExisting\\*.dll"; //test handling a non-existing path
            resultArray = inputPath.ExpandWildcardPath();
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(0, resultArray.Length);

            inputPath = "%bytes.net.dir%\\..\\..\\Debug\\net*\\";
            resultArray = inputPath.ExpandWildcardPath();
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(4, resultArray.Length);

            inputPath = "%bytes.net.dir%\\..\\..\\Debug\\n*72";
            resultArray = inputPath.ExpandWildcardPath();
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(1, resultArray.Length);

            inputPath = "%bytes.net.dir%\\..\\..\\D*ug\\net*\\";
            resultArray = inputPath.ExpandWildcardPath();
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(4, resultArray.Length);

            //check expansion for custom variables
            string varValue = "%bytes.net.dir%";
            Dictionary<string, string> variables = new Dictionary<string, string>() { { "demo.variable", varValue.ExpandPath() } };

            inputPath = "%demo.variable%\\..\\..\\Debug\\net472\\*.dll";
            resultArray = inputPath.ExpandWildcardPath(variables);
            Debug.WriteLine("Expanding variable(s) in '" + inputPath + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(10, resultArray.Length);

            string[] inputArray = new string[]
            {
                "%bytes.net.dir%\\..\\..\\Debug\\net472\\*.dll",
                "%demo.variable%\\..\\..\\Debug\\net472\\*.dll"
            };
            resultArray = inputArray.ExpandWildcardPath(variables);
            Debug.WriteLine("Expanding variable(s) in '" + System.String.Join(",", inputArray) + "' resulted in '" + System.String.Join(",", resultArray) + "'");
            Assert.AreEqual(10, resultArray.Length);
        }
    }
}
