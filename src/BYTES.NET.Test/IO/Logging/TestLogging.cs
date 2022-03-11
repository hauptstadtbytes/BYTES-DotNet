//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Logging;
using BYTES.NET.IO;

//import namespace(s) required from 'BYTES.NET.MS' framework
using BYTES.NET.MS.IO.Logging;

namespace BYTES.NET.Test.IO.Logging
{

    [TestClass]
    public class TestLogging
    {

        int _counter = 0;

        [TestMethod]
        public void TestEventHandling()
        {
            Log myLog = new Log();
            myLog.Logged += OnLogged;

            for (int i = 1; i <= 3; i++)
            {
                myLog.Inform("Info# " + i.ToString());
            }

            Assert.AreEqual(3, _counter);
            Assert.AreEqual(_counter,myLog.GetCache().Length);

        }

        [TestMethod]
        public void TestThresholding()
        {
            LogEntry myEntry = new LogEntry("A sample information for testing", LogEntry.InformationLevel.Info);
            Assert.AreEqual(true, myEntry.MatchesThreshold(LogEntry.InformationLevel.Debug));
            Assert.AreEqual(true, myEntry.MatchesThreshold(LogEntry.InformationLevel.Info));
            Assert.AreEqual(false, myEntry.MatchesThreshold(LogEntry.InformationLevel.Warning));
            Assert.AreEqual(false, myEntry.MatchesThreshold(LogEntry.InformationLevel.Exception));
            Assert.AreEqual(false, myEntry.MatchesThreshold(LogEntry.InformationLevel.Fatal));

            myEntry = new LogEntry("A sample information for testing", LogEntry.InformationLevel.Exception);
            Assert.AreEqual(true, myEntry.MatchesThreshold(LogEntry.InformationLevel.Warning));
            Assert.AreEqual(false, myEntry.MatchesThreshold(LogEntry.InformationLevel.Fatal));
        }

        [TestMethod]
        public void TestLog()
        {
            Log myLog = new Log("testLogIdentifyer") { Threshold = LogEntry.InformationLevel.Warning };
            myLog.Logged += OnLogged;

            myLog.Warn("A sample warning");
            Assert.AreEqual(1,myLog.GetCache().Length);

            myLog.Trace("A sample debug message - should not be visible");
            Assert.AreEqual(1, myLog.GetCache().Length);

            myLog.Inform("A sample information - should not be visible");
            Assert.AreEqual(1, myLog.GetCache().Length);

            myLog.AddEntry("A sample error", LogEntry.InformationLevel.Fatal);
            Assert.AreEqual(2, myLog.GetCache().Length);
        }

        [TestMethod]
        public void TestRollingFileAppender()
        {
            string filePath = Helper.ExpandPath("%bytes.net.dir%\\..\\..\\..\\..\\..\\test\\Logs\\");

            //check the default behavior
            if (File.Exists(filePath + "RollingFileLog-Dumped.LOG"))
            {
                File.Delete(filePath + "RollingFileLog-Dumped.LOG");
            }

            Log myFirstLog = new Log();
            myFirstLog.Inform("Log initialized (w/o appender)");

            myFirstLog.AddAppender(new RollingFileAppender(filePath + "RollingFileLog-Dumped.LOG"));
            myFirstLog.Inform("Rolling file appender added");

            Assert.AreEqual(4, Helper.ReadAllAllLines(filePath + "RollingFileLog-Dumped.LOG").Length);

            myFirstLog.Trace("A trace message - should not be visible");
            Assert.AreEqual(4, Helper.ReadAllAllLines(filePath + "RollingFileLog-Dumped.LOG").Length);

            //check for the dumping behavior
            if (File.Exists(filePath + "RollingFileLog-NotDumped.LOG"))
            {
                File.Delete(filePath + "RollingFileLog-NotDumped.LOG");
            }

            Log mySecondLog = new Log() { Threshold = LogEntry.InformationLevel.Debug };
            mySecondLog.AddAppender(new RollingFileAppender(filePath + "RollingFileLog-NotDumped.LOG", new System.Collections.Generic.Dictionary<string, object>() { { "dumponattachement", false } }));
            Assert.AreEqual(0, Helper.ReadAllAllLines(filePath + "RollingFileLog-NotDumped.LOG").Length);

            mySecondLog.Trace("A trace message"); //a known BUG: The message is also logged in the first log file (due to the way Log4Net is used?)
            Assert.AreEqual(1, Helper.ReadAllAllLines(filePath + "RollingFileLog-NotDumped.LOG").Length);
        }

        private void OnLogged(ref LogEntry entry)
        {
            _counter++;
            Trace.WriteLine(entry.ToString());
        }

    }
}
