//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

using System;
using System.IO;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Collections;
using BYTES.NET.IO;
using BYTES.NET.IO.Persistance.Extensions;

namespace BYTES.NET.Test.Collections
{
    [TestClass]
    public class TestExtendedDictionary
    {

        [TestMethod]
        public void TestDictionary()
        {
            ExtendedDictionary<string, string> dic = new ExtendedDictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "First", "Hello" }, { "Second", "World" } };

            //validate the dictionary feature(s)
            dic.Add("Third", "!");
            Assert.AreEqual(3, dic.Count);
            Assert.AreEqual("HelloWorld!", dic["First"] + dic["second"] + dic["third"]);
        }

        [TestMethod]
        public void TestWriteReadDefaultXML()
        {

            string filePath = "%BYTES.NET.DIR%\\..\\..\\..\\..\\..\\test\\Dictionary\\sampleStringDic.XML";
            filePath = Helper.ExpandPath(filePath);

            if (File.Exists(filePath)){
                File.Delete(filePath);
            }

            ExtendedDictionary<string,string> dic1 = new ExtendedDictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "First", "Hello" }, { "Second", "World" }, { "Third", "!" } };

            //write to XML file
            dic1.WriteToXML(filePath);
            Assert.AreEqual(true,File.Exists(filePath));

            //read from XML file
            dic1.ReadFromXML(filePath);
            Assert.AreEqual(3, dic1.Count);
            Assert.AreEqual("HelloWorld!", dic1["First"] + dic1["second"] + dic1["third"]);

        }

        [TestMethod]
        public void TestInheritance()
        {
            //create the dictionary
            ListDictionary myList = new ListDictionary() { {1,"One" }, {2, "Two" } };
            myList.Add(3, "Three");

            Assert.AreEqual(3, myList.Count);

            //write to disk file
            string filePath = "%BYTES.NET.DIR%\\..\\..\\..\\..\\..\\test\\Dictionary\\sampleIntDic.XML";
            filePath = Helper.ExpandPath(filePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            myList.WriteToXML(filePath);
            Assert.AreEqual(true, File.Exists(filePath));

            //read from disk file
            ListDictionary clonedList = new ListDictionary();
            clonedList.ReadFromXML(filePath);

            Assert.AreEqual(3,clonedList.Count);
            Assert.AreEqual("Two", clonedList[2]);
        }

    }
}
