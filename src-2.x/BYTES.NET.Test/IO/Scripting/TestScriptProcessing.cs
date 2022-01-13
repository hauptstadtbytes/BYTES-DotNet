//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

using System;
using System.IO;
using System.Collections.Generic;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Scripting;
using BYTES.NET.IO;
using BYTES.NET.IO.Persistance.Extensions;
using BYTES.NET.IO.Logging;

//import internal namespace(s) required
using BYTES.NET.Test.IO.Scripting.Methods;

namespace BYTES.NET.Test.IO.Scripting
{
    [TestClass]
    public class TestScriptProcessing
    {
        private string filePath = "%BYTES.NET.DIR%\\..\\..\\..\\..\\..\\test\\Scripting\\sampleScript.XML";

        [TestMethod]
        public void TestWriteReadScript()
        {
            //setup the testing environment
            filePath = Helper.ExpandPath(filePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            //assemble the script
            TestScript script = new TestScript();

            script.Name = "Sample Script";
            script.Description = "As simple script for testing";

            script.Sequence.Calls.Add(new MethodCall() { Method = "SetVariable", Arguments = new MethodCallArguments() { { "Name", "Hello" }, { "Value", "World" } } });
            script.Sequence.Calls.Add(new MethodCall() { Method = "LogMessage", Arguments = new MethodCallArguments() { { "Message", "Let's do a 'Hello %Hello%'" }, { "Level", "Fatal" } } });
            script.Sequence.Calls.Add(new MethodCall() { Method = "Write", Arguments = new MethodCallArguments() });

            //write to XML file
            script.WriteToXML(filePath);
            Assert.AreEqual(true, File.Exists(filePath));

            //read from XML file
            script.ReadFromXML(filePath);
            Assert.AreEqual(3, script.Sequence.Calls.Count);
            Assert.AreEqual("SetVariable", script.Sequence.Calls[0].Method);

        }

        [TestMethod]
        public void TestScriptExecution()
        {
            //setup the testing environment
            filePath = Helper.ExpandPath(filePath);

            if (!File.Exists(filePath))
            {
                File.Delete(filePath);
                TestWriteReadScript();
            }

            //enumerate the method(s)
            Dictionary<string, API.ITestMethod> methods = new Dictionary<string, API.ITestMethod>(StringComparer.OrdinalIgnoreCase);
            methods.Add("SetVariable", new SetVariable());
            methods.Add("LogMessage", new LogMessage());
            methods.Add("Write", new WriteFile());

            //setup the context
            TestExecutionContext context = new TestExecutionContext() { Methods = methods };
            context.MessageReceived += OnLogged;

            //load the script
            TestScript script = new TestScript();
            script.ReadFromXML(filePath);

            //execute the script
            ExecutionResult result = context.Execute(script.Sequence);
            Assert.AreEqual(true, result.Successful);
        }

        private void OnLogged(ref LogEntry entry)
        {
            Trace.WriteLine(entry.ToString());
        }
    }
}
