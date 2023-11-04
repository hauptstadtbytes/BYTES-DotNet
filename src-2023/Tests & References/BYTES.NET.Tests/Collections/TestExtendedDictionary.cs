//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.IO;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Collections;
using BYTES.NET.IO;
using BYTES.NET.Tests.Extensibility.API;
using System.Diagnostics;
using BYTES.NET.Persistance;

namespace BYTES.NET.Tests.Collections
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
        public void TestIndexingAndFiltering()
        {
            //test primitive types
            ExtendedDictionary<string, string> dic1 = new ExtendedDictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "First", "Hello" }, { "Second", "World" } };
            dic1.Add("Third", "!");

            Dictionary<string, List<string>> indexed = dic1.IndexByItemProperty<string>("Length"); //index by string lenght
            Assert.AreEqual(2, indexed.Count);
            Assert.AreEqual(2, indexed["5"].Count);
            Assert.AreEqual(1, indexed["1"].Count);

            List<String> filtered = dic1.FilterByItemProperty<string>("Length", "1"); //filter for all items with length = 1
            Assert.AreEqual(1, filtered.Count);

            filtered = dic1.FilterByItemProperty<int>("Length", 1); //filter for all items with length = 1
            Assert.AreEqual(1, filtered.Count);

            //test complex types
            ExtendedDictionary<string, SampleMetadata> dic2 = new ExtendedDictionary<string, SampleMetadata>();
            SampleMetadata first = new SampleMetadata() { Name = "My first", Description = "Lyrics" };
            dic2.Add("One", first);
            dic2.Add("Two", new SampleMetadata() { Name = "My last", Description = "Lyrics" });
            dic2.Add("Third", new SampleMetadata() { Name = "My Everything", Description = "Lyrics" });

            Dictionary<string, List<SampleMetadata>> indexed2 = dic2.IndexByItemProperty<string>("Description");
            Assert.AreEqual(1, indexed2.Count);

            //foreach(string key in aggregated3.Keys)
            //{
                //Debug.WriteLine(key);
            //}

            Assert.AreEqual(3, indexed2["Lyrics"].Count);

            //recursive properties are currently not supported
        }

        [TestMethod]
        public void TestInheritanceAndSerialization()
        {
            //create the dictionary
            SampleDictonaryChild myList = new SampleDictonaryChild() { { 1, "One" }, { 2, "Two" } };
            myList.Add(3, "Three");

            Assert.AreEqual(3, myList.Count);

            //write to disk file
            string filePath = "%BYTES.NET.DIR%\\..\\..\\..\\..\\..\\Sample Data\\sampleIntDic.XML";
            filePath = filePath.ExpandPath();

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            myList.WriteToXML(filePath);
            Assert.AreEqual(true, File.Exists(filePath));

            //read from disk file
            SampleDictonaryChild clonedList = new SampleDictonaryChild();
            clonedList.ReadFromXML(filePath);

            Assert.AreEqual(3, clonedList.Count);
            Assert.AreEqual("Two", clonedList[2]);
        }
    }
}
