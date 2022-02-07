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
            TestCollection source = new TestCollection("Hello World!") { Number = 42 };

            source.WriteToXML(thePath);
            Trace.WriteLine("Written to '" + thePath + "'");
            Assert.AreEqual(true, File.Exists(thePath));

            //read from XML
            TestCollection destination = new TestCollection();
            destination.ReadFromXML(thePath);
            Trace.WriteLine("Read from '" + thePath + "'");

            Assert.AreEqual("Hello World!", destination.Text);
            Assert.AreEqual(42, destination.Number);
        }
    }
}
