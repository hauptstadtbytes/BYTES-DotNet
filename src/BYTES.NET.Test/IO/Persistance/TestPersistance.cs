//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Persistance.Extensions;
using BYTES.NET.IO;

namespace BYTES.NET.Test.IO.Persistance
{

    [TestClass]
    public class TestPersistance
    {
        private string filePath = "%BYTES.NET.DIR%\\..\\..\\..\\..\\..\\test\\Persistance\\sampleData";

        [TestMethod]
        public void TestXMLWriteAndRead()
        {
            string thePath = Helper.ExpandPath(filePath + ".xml");

            //cleanup the test environment
            if (File.Exists(thePath))
            {
                File.Delete(thePath);
            }

            //write to XML
            TestObject testObj = new TestObject() { ID = 42, Name = "Hello World!", Group = "Sample 1" };

            testObj.WriteToXML(thePath);
            Debug.WriteLine("Written to '" + thePath + "'");
            Assert.AreEqual(true, File.Exists(thePath));

            //read from XML
            TestObject loadedTestObj = new TestObject();
            loadedTestObj.ReadFromXML(thePath);
            Debug.WriteLine("Read from '" + thePath + "'");

            Assert.AreEqual("Hello World!", loadedTestObj.Name);
            Assert.AreEqual(42, loadedTestObj.ID);
        }

        [TestMethod]
        public void TestCSVWriteAndRead()
        {
            string thePath = Helper.ExpandPath(filePath + ".csv");

            //cleanup the test environment
            if (File.Exists(thePath))
            {
                File.Delete(thePath);
            }

            //write to CSV
            TestObject parentObj = new TestObject() { ID = 99, Name = "Origin", Group = "Sample 1" };
            TestObject testObj = new TestObject(parentObj) { ID = 42, Name = "Hello World!", Group = "Sample 1" };

            testObj.WriteToCSV(thePath);
            Debug.WriteLine("Written to '" + thePath + "'");
            Assert.AreEqual(true, File.Exists(thePath));

            //read from CSV
            TestObject loadedTestObj = new TestObject();
            loadedTestObj.ReadFromCSV(thePath);
            Debug.WriteLine("Read from '" + thePath + "'");

            Assert.AreEqual("Hello World!", loadedTestObj.Name);
            Assert.AreEqual(42, loadedTestObj.ID);
            Assert.AreEqual(99, loadedTestObj.Parent.ID);
        }
    }
}
