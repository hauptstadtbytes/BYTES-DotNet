//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Persistance;
using BYTES.NET.IO;

namespace BYTES.NET.Tests.Persistable
{
    [TestClass]
    public class TestPersistables
    {
        private string filePath = "%BYTES.NET.DIR%\\..\\..\\..\\..\\..\\Sample Data\\samplePersistable";

        [TestMethod]
        public void TestXMLSerializationAndDeserialization()
        {
            string thePath = filePath + ".xml";
            thePath = thePath.ExpandPath();

            //cleanup the test environment
            if (File.Exists(thePath))
            {
                File.Delete(thePath);
            }

            //write to XML
            SamplePersistableobject testObj = new SamplePersistableobject() { ID = 42, Name = "Hello World!", Group = "Sample 1" };

            testObj.WriteToXML(thePath);
            Debug.WriteLine("Written to '" + thePath + "'");
            Assert.AreEqual(true, File.Exists(thePath));

            //read from XML (not implemented in the 'SamplePersistableObject'; see 'TestExtendedDictionary' instead)
            //SamplePersistableobject loadedTestObj = new SamplePersistableobject();
            //loadedTestObj.ReadFromXML(thePath);
            //Debug.WriteLine("Read from '" + thePath + "'");

            //Assert.AreEqual("Hello World!", loadedTestObj.Name);
            //Assert.AreEqual(42, loadedTestObj.ID);

        }

        [TestMethod]
        public void TestCSVWritingAndReading()
        {
            string thePath = filePath + ".csv";
            thePath = thePath.ExpandPath();

            //cleanup the test environment
            if (File.Exists(thePath))
            {
                File.Delete(thePath);
            }

            //write to CSV
            SamplePersistableobject parentObj = new SamplePersistableobject() { ID = 99, Name = "Origin", Group = "Sample 1" };
            SamplePersistableobject testObj = new SamplePersistableobject(parentObj) { ID = 42, Name = "Hello World!", Group = "Sample 1" };

            testObj.WriteToCSV(thePath);
            Debug.WriteLine("Written to '" + thePath + "'");
            Assert.AreEqual(true, File.Exists(thePath));

            //read from CSV
            SamplePersistableobject loadedTestObj = new SamplePersistableobject();
            loadedTestObj.ReadFromCSV(thePath);
            Debug.WriteLine("Read from '" + thePath + "'");

            Assert.AreEqual("Hello World!", loadedTestObj.Name);
            Assert.AreEqual(42, loadedTestObj.ID);
            Assert.AreEqual(99, loadedTestObj.Parent.ID);
        }
    }
}
