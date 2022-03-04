//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Logging;
using BYTES.NET.IO;
using System.Collections.Generic;

//import namespace(s) required from 'BYTES.NET.MS' framework
using BYTES.NET.MS.IO.Logging;

namespace BYTES.NET.Test.IO.Logging
{

    [TestClass]
    public class TestRollingFileAppender
    {
        [TestMethod]
        public void TestFileLogging()
        {

            string filePath = "%bytes.net.dir%\\..\\..\\..\\..\\..\\test\\Logs\\RollingFileAppenderTest.LOG";

            Log myLog = new Log();
            myLog.Inform("log file created");

            if (File.Exists(Helper.ExpandPath(filePath)))
            {
                File.Delete(Helper.ExpandPath(filePath));
            }

            myLog.AddAppender(new RollingFileAppender(filePath));
            myLog.Inform("rolling file appender added");

            List<string> lines = new List<string>();
            using (FileStream fs = new FileStream(Helper.ExpandPath(filePath), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }

            Assert.AreEqual(4, lines.Count);
        }

    }
}
