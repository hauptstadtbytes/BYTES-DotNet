//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;

//import namespace(s) required from 'BYTES.NET' library
using BYTES.NET.Collections;

namespace BYTES.NET.TEST.Collections
{

    [TestClass]
    public class TestXMLSerializableDictionary
    {

        private string testDir = "C:\\UnitTesting";

        private XMLSerializableDictionary<string, string> dic1 = new XMLSerializableDictionary<string, string>() { { "first", "hello" }, { "second", "world" }, { "third", "!" }, { "fourth", "another" } };
        private XMLSerializableDictionary<string, SampleDataType> dic2 = new XMLSerializableDictionary<string, SampleDataType>();

        [TestMethod]
        public void WriteReadNonSerializedDictionaryFile()
        {

            //write the dictionary to disk file
            if(!Directory.Exists(testDir))
            {

                Directory.CreateDirectory(testDir);

            }

            FileStream outFile = new FileStream(testDir + "\\Dictionary1.xml", FileMode.OpenOrCreate);

            dic1.Write(ref outFile);

            //check if the file exists
            Assert.IsTrue(File.Exists(testDir + "\\Dictionary1.xml"));

            //read the dictionary from disk file
            FileStream inFile = new FileStream(testDir + "\\Dictionary1.xml", FileMode.Open);

            //read the disk file
            XMLSerializableDictionary<string, string> readDic = new XMLSerializableDictionary<string, string>();
            readDic = readDic.Read(ref inFile);

            //check for the dictionary content
            Assert.AreEqual(readDic.Count, dic1.Count); //check for the total number of items
            Assert.AreEqual(readDic["first"].ToString(), dic1["first"].ToString()); //check the first item
            Assert.AreEqual(readDic["fourth"].ToString(), dic1["fourth"].ToString()); //check the fourth item

            //delete the disk file
            //File.Delete(testDir + "\\Dictionary1.xml");

        }

        [TestMethod]
        public void WriteReadSerializedDictionaryFile()
        {

            //update the dictionary
            dic2 = new XMLSerializableDictionary<string, SampleDataType>();
            dic2.Add("first", new SampleDataType());
            dic2.Add("second", new SampleDataType() { Counter = 99, Description = "This was programmatically changed" });

            //write the dictionary to disk file
            if (!Directory.Exists(testDir))
            {

                Directory.CreateDirectory(testDir);

            }

            FileStream outFile = new FileStream(testDir + "\\Dictionary2.xml", FileMode.OpenOrCreate);

            dic2.Write(ref outFile);

            //check if the file exists
            Assert.IsTrue(File.Exists(testDir + "\\Dictionary2.xml"));

            //read the dictionary from disk file
            FileStream inFile = new FileStream(testDir + "\\Dictionary2.xml", FileMode.Open);

            XMLSerializableDictionary<string, SampleDataType> readDic = new XMLSerializableDictionary<string, SampleDataType>();
            readDic = readDic.Read(ref inFile);

            //check for the dictionary content
            Assert.AreEqual(readDic.Count, dic2.Count); //check for the total number of items
            Assert.AreEqual(readDic["first"].Counter.ToString(), dic2["first"].Counter.ToString()); //check the first item
            Assert.AreEqual(readDic["second"].Description, dic2["second"].Description); //check the second item

            //delete the disk file
            //File.Delete(testDir + "\\Dictionary2.xml");

        }

        [TestMethod]
        public void WriteReadInheritedSerializedDictionaryFile()
        {

            //update the dictionary
            SampleInheritedDictionary dic3 = new SampleInheritedDictionary();
            dic3.Add("first", new SampleDataType());
            dic3.Add("42", new SampleDataType() { Counter = 99, Description = "This was programmatically changed" });

            //write the dictionary to disk file
            if (!Directory.Exists(testDir))
            {

                Directory.CreateDirectory(testDir);

            }

            FileStream outFile = new FileStream(testDir + "\\Dictionary3.xml", FileMode.OpenOrCreate);

            dic3.Write(ref outFile);

            //check if the file exists
            Assert.IsTrue(File.Exists(testDir + "\\Dictionary3.xml"));

            //read the dictionary from disk file
            SampleInheritedDictionary readDic = new SampleInheritedDictionary();
            readDic.Read(testDir + "\\Dictionary3.xml");

            //check for the dictionary content
            Assert.AreEqual(readDic.Count, dic3.Count); //check for the total number of items
            //Assert.AreEqual(readDic[1].Counter.ToString(), dic3[1].Counter.ToString()); //check the first item
            //Assert.AreEqual(readDic[42].Description, dic3[42].Description); //check the second item

            //delete the disk file
            //File.Delete(testDir + "\\Dictionary3.xml");

        }

    }

}
