//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Automation;
using BYTES.NET.Automation.Scripting;
using BYTES.NET.IO;
using BYTES.NET.Persistance;

namespace BYTES.NET.Tests.Automation
{
    [TestClass]
    public class TestPackageReadingWriting
    {
        private string dirPathPath = "%BYTES.NET.DIR%\\..\\..\\..\\..\\..\\Sample Data\\";

        [TestMethod]
        public void TestWritingReading()
        {
            //create a sample package
            Package myPackage = new Package();

            //create a sample script
            Script myScript = new Script();
            myScript.Metadata.Name = "a test script";
            myScript.Metadata.Description = "a simple script for testing purposes";

            //add the script to the package
            myPackage.Scripts.Add(myScript);

            string filePath = dirPathPath + "SamplePackage.apkx";
            filePath = filePath.ExpandPath();

            PrepareEnvironment(filePath);

            //check if the file is written to disk
            myPackage.Write(filePath);
            Assert.IsTrue(File.Exists(filePath));

            //add another script to the package (update)
            myPackage.Scripts.Add(new Script());

            //check if the (updated) file is written to disk
            myPackage.Write(filePath);
            Assert.IsTrue(File.Exists(filePath));
        }

        private void PrepareEnvironment(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
