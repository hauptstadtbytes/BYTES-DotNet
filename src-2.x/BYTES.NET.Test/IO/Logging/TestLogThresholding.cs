//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Logging;


namespace BYTES.NET.Test.IO.Logging
{
    [TestClass]
    public class TestLogThresholding
    {
        [TestMethod]
        public void TestThresholding()
        {
            LogEntry myEntry = new LogEntry("A sample information for testing",LogEntry.InformationLevel.Info);
            Assert.AreEqual(true, myEntry.MatchesThreshold(LogEntry.InformationLevel.Debug));
            Assert.AreEqual(true, myEntry.MatchesThreshold(LogEntry.InformationLevel.Info));
            Assert.AreEqual(false, myEntry.MatchesThreshold(LogEntry.InformationLevel.Warning));
            Assert.AreEqual(false, myEntry.MatchesThreshold(LogEntry.InformationLevel.Exception));
            Assert.AreEqual(false, myEntry.MatchesThreshold(LogEntry.InformationLevel.Fatal));

            myEntry = new LogEntry("A sample information for testing", LogEntry.InformationLevel.Exception);
            Assert.AreEqual(true, myEntry.MatchesThreshold(LogEntry.InformationLevel.Warning));
            Assert.AreEqual(false, myEntry.MatchesThreshold(LogEntry.InformationLevel.Fatal));
        }

    }
}
