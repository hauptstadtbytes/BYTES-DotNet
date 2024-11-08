//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Automation.Scripting;
using BYTES.NET.IO;
using BYTES.NET.Persistance;

namespace BYTES.NET.Tests.Automation
{
    [TestClass]
    public class TestScriptReadingWriting
    {
        private string dirPathPath = "%BYTES.NET.DIR%\\..\\..\\..\\..\\..\\Sample Data\\";

        [TestMethod]
        public void TestWritingReading()
        {
            //create the script (root) instance
            Script myScript = new Script();
            myScript.Metadata.Name = "a test script";
            myScript.Metadata.Description = "a simple script for testing purposes";

            //update the root sequence metadata
            //myScript.Sequences.First().Metadata.Name = "First Sequence";
            //myScript.Sequences.Find(x => x.ID.ToString().Equals(myScript.Arguments["RootSequence"])).Metadata.Description = "Another Description";

            Sequence rootSeq = myScript.Sequences.Find(x => x.ID.ToString().Equals(myScript.Arguments["RootSequence"]));
            //rootSeq.Calls.Add(new Call());

            string filePath = dirPathPath + "SampleScript.scrptx";
            filePath = filePath.ExpandPath();

            PrepareEnvironment(filePath);

            //check if the file is written to disk
            myScript.WriteToXML(filePath);
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
