//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Logging;

namespace BYTES.NET.Tests.Logging
{
    [TestClass]
    public class TestLogging
    {
        int _counter = 0;

        [TestMethod]
        public void TestLog()
        {
            Log myLog = new Log("testLogIdentifyer") { Threshold = LogEntry.InformationLevel.Warning };
            myLog.Logged += OnLogged;

            myLog.Warn("A sample warning");
            Assert.AreEqual(1, myLog.GetCache().Length);

            myLog.Trace("A sample debug message - should not be visible");
            Assert.AreEqual(1, myLog.GetCache().Length);

            myLog.Inform("A sample information - should not be visible");
            Assert.AreEqual(1, myLog.GetCache().Length);

            myLog.Write("A sample error", LogEntry.InformationLevel.Fatal);
            Assert.AreEqual(2, myLog.GetCache().Length);
        }

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
            Assert.AreEqual(_counter, myLog.GetCache().Length);

        }

        [TestMethod]
        public void TestThresholding()
        {
            LogEntry myEntry = new LogEntry("A sample information for testing", LogEntry.InformationLevel.Info);
            Assert.AreEqual(true, myEntry.IsMoreImportant(LogEntry.InformationLevel.Debug));
            Assert.AreEqual(true, myEntry.IsMoreImportant(LogEntry.InformationLevel.Info));
            Assert.AreEqual(false, myEntry.IsMoreImportant(LogEntry.InformationLevel.Warning));
            Assert.AreEqual(false, myEntry.IsMoreImportant(LogEntry.InformationLevel.Exception));
            Assert.AreEqual(false, myEntry.IsMoreImportant(LogEntry.InformationLevel.Fatal));

            myEntry = new LogEntry("A sample information for testing", LogEntry.InformationLevel.Exception);
            Assert.AreEqual(true, myEntry.IsMoreImportant(LogEntry.InformationLevel.Warning));
            Assert.AreEqual(false, myEntry.IsMoreImportant(LogEntry.InformationLevel.Fatal));
        }

        private void OnLogged(ref LogEntry entry)
        {
            _counter++;
            Debug.WriteLine(entry.ToString());
        }
    }
}
