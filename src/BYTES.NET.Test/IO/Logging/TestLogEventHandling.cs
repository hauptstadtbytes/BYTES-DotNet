//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Logging;

namespace BYTES.NET.Test.IO.Logging
{

    [TestClass]
    public class TestLogEventHandling
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

        private void OnLogged(ref LogEntry entry)
        {
            _counter++;
        }

    }
}
