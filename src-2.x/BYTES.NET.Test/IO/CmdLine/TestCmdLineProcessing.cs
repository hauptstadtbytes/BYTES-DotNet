//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.CmdLine;
using BYTES.NET.IO.Logging;

//import internal namespace(s) required
using BYTES.NET.Test.IO.CmdLine;

namespace BYTES.NET.Test.IO.Cmdline
{

    [TestClass]
    public class TestCmdLineProcessing
    {

        private LogEntry _lastMessage = new LogEntry(string.Empty);

        [TestMethod]
        public void TestArgsParsing()
        {
            string[] testArgs = { "myMethod", "-path", "\"C:\\Sample\"", "/switch", "-key=value", "-anotherKey:AnotherValue", "/secondPath:\"C:\\Test\\2\"", "/?"};

            CmdLineArguments testCollection = new CmdLineArguments(testArgs);

            Assert.AreEqual(6, testCollection.Keys.Count);

            Assert.AreEqual(string.Empty, testCollection["switch"]);
            Assert.AreEqual(true,testCollection.ContainsKey("switch"));
            Assert.AreEqual(true, testCollection.ContainsKey("?"));

            Assert.AreEqual("value", testCollection["key"]);
            Assert.AreEqual("AnotherValue", testCollection["anotherKey"]);

            Assert.AreEqual("C:\\Sample", testCollection["path"]);
            Assert.AreEqual("C:\\Test\\2", testCollection["secondPath"]);

            Assert.AreEqual(false, testCollection.IsNamedArgument(0));

        }

        [TestMethod]
        public void TestGlobalHelpRequest()
        {
            CmdLineExecutionContext context = new CmdLineExecutionContext();
            context.Methods.Add("WriteMessage", new ReturnMessage());
            context.MessageReceived += HandleMessageReceived;

            context.Execute(new string[] { });
            Assert.AreEqual("WriteMessage - Writes a message (e.g. to console)", _lastMessage.Message);
        }

        [TestMethod]
        public void TestMethodHelpRequest()
        {
            CmdLineExecutionContext context = new CmdLineExecutionContext();
            context.Methods.Add("WriteMessage", new ReturnMessage());
            context.MessageReceived += HandleMessageReceived;

            context.Execute(new string[] {"writemessage","/?"});
            Assert.AreEqual("[-uppercase] Makes all characters upper case", _lastMessage.Message);
        }

        [TestMethod]
        public void TestArgsValidation()
        {
            CmdLineExecutionContext context = new CmdLineExecutionContext();
            context.Methods.Add("WriteMessage", new ReturnMessage());
            context.MessageReceived += HandleMessageReceived;

            context.Execute(new string[] { "writemessage", "/not:working" });
            Assert.AreEqual("[-uppercase] Makes all characters upper case", _lastMessage.Message);
        }

        [TestMethod]
        public void TestProcessing()
        {
            CmdLineExecutionContext context = new CmdLineExecutionContext();
            context.Methods.Add("WriteMessage", new ReturnMessage());
            context.MessageReceived += HandleMessageReceived;

            context.Execute(new string[] { "writemessage", "/msg:HelloWorld!" });
            Assert.AreEqual("HelloWorld!", _lastMessage.Message);

            context.Execute(new string[] { "writemessage", "/msg:HelloWorld!", "/uppercase" });
            Assert.AreEqual("HELLOWORLD!", _lastMessage.Message);

            context.Execute(new string[] { "writemessage", "/msg:\"Hello World!\"", "/uppercase" });
            Assert.AreEqual("HELLO WORLD!", _lastMessage.Message);
        }

        private void HandleMessageReceived(ref LogEntry message)
        {
            Trace.WriteLine(message);
            _lastMessage = message;
        }

    }
}
