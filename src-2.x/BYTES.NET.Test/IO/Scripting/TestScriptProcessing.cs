//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

using System;
using System.IO;
using System.Collections.Generic;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Scripting;
using BYTES.NET.IO.Scripting.API;
using BYTES.NET.IO.Scripting.Methods;
using BYTES.NET.IO;
using BYTES.NET.IO.Persistance.Extensions;
using BYTES.NET.IO.Logging;

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
            Script script = new Script();
            script.Sequence.Add(new MethodCall() { MethodID = "SetVariable", Arguments = new MethodCallArguments() { { "Name", "Hello" }, { "Value", "World" } } });
            script.Sequence.Add(new MethodCall() { MethodID = "LogMessage", Arguments = new MethodCallArguments() { { "Message", "Let's do a 'Hello %Hello%'" }, { "Level", "Fatal" } } });
            script.Sequence.Add(new MethodCall() { MethodID = "Write", Arguments = new MethodCallArguments() });

            //write to XML file
            script.WriteToXML(filePath);
            Assert.AreEqual(true, File.Exists(filePath));

            //read from XML file
            script.ReadFromXML(filePath);
            Assert.AreEqual(3, script.Sequence.Count);
            Assert.AreEqual("SetVariable", script.Sequence[0].MethodID);

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
            Dictionary<string, IMethod> methods = new Dictionary<string, IMethod>(StringComparer.OrdinalIgnoreCase);
            methods.Add("SetVariable", new SetVariable());
            methods.Add("LogMessage", new LogMessage());
            methods.Add("Write", new Extensibility.WriteLog());

            //setup the context
            ScriptExecutionContext context = new ScriptExecutionContext() { Methods = methods };
            context.MessageReceived += OnLogged;

            //load the script
            Script script = new Script();
            script.ReadFromXML(filePath);

            //execute the script
            ScriptExecutionResult result = context.Execute(script);
            Assert.AreEqual(true, result.Successful);
        }

        private void OnLogged(ref LogEntry entry)
        {
            Trace.WriteLine(entry.ToString());
        }
    }
}
