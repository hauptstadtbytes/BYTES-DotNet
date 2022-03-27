//import .net namespace(s) required
using System.Collections.Generic;
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

            //test the (basic) dictionary feature(s)
            dic.Add("Third", "!");
            Assert.AreEqual(3, dic.Count);
            Assert.AreEqual("HelloWorld!", dic["First"] + dic["second"] + dic["third"]);
        }

        [TestMethod]
        public void TestIndexing()
        {
            //test primitive types
            ExtendedDictionary<string, string> dic1 = new ExtendedDictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "First", "Hello" }, { "Second", "World" } };
            dic1.Add("Third", "!");

            Dictionary<string, List<string>> aggregated1 = dic1.ByItemProperty<string>("Length");
            Assert.AreEqual(2, aggregated1.Count);
            Assert.AreEqual(2, aggregated1["5"].Count);
            Assert.AreEqual(1, aggregated1["1"].Count);

            aggregated1 = dic1.ByItemProperty<string>("Length","1"); //filter for all items with length = 1
            Assert.AreEqual(1, aggregated1.Count);
            Assert.AreEqual(1, aggregated1["1"].Count);

            Dictionary<int, List<string>> aggregated2 = dic1.ByItemProperty<int>("Length");
            Assert.AreEqual(2, aggregated2.Count);
            Assert.AreEqual(2, aggregated2[5].Count);

            aggregated2 = dic1.ByItemProperty<int>("Length",5); //filter for all items with length = 5
            Assert.AreEqual(1, aggregated2.Count);
            Assert.AreEqual(2, aggregated2[5].Count);

            //test complex types
            ExtendedDictionary<string, TestObject> dic2 = new ExtendedDictionary<string,TestObject>();
            TestObject first = new TestObject() { Name = "My first" , Indexer = 1, Group="Lyrics"};
            dic2.Add("One", first);
            dic2.Add("Two", new TestObject() { Name="My last", Indexer = 2, Parent = first, Group = "Lyrics" });
            dic2.Add("Third", new TestObject() { Name = "My Everything", Indexer = 3, Group = "Lyrics" });

            Dictionary<string, List<TestObject>> aggregated3 = dic2.ByItemProperty<string>("Group");
            Assert.AreEqual(1, aggregated3.Count);
            Assert.AreEqual(3, aggregated3["Lyrics"].Count);

            //case-insensitivity is not supported by now (for property name as well as for output key)
            //aggregated3 = dic2.ByItemProperty<string>("group");
            //Assert.AreEqual(3, aggregated3["lyrics"].Count);

            aggregated3 = dic2.ByItemProperty<string>("Parent");
            Assert.AreEqual(2, aggregated3.Count);
            Assert.AreEqual(2, aggregated3["%BYTES.Empty%"].Count);

            //recursive properties are currently not supported
            //aggregated3 = dic2.ByItemProperty<string>("Parent.Name");
            //Assert.AreEqual(1, aggregated3.Count);

            Dictionary<int, List<TestObject>> aggregated4 = dic2.ByItemProperty<int>("Indexer");
            Assert.AreEqual(3, aggregated4.Count);

            Dictionary<TestObject, List<TestObject>> aggregated5 = dic2.ByItemProperty<TestObject>("Parent");
            Assert.AreEqual(1, aggregated5.Count);
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
