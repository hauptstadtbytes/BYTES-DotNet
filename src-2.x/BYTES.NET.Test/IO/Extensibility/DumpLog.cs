//import .net namespace(s) required
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import .net namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Scripting;
using BYTES.NET.IO.Scripting.API;
using BYTES.NET.IO;

//import .net namespace(s) required from 'BYTES.NET.MS' framework
using BYTES.NET.MS.IO.Logging;

namespace BYTES.NET.Test.IO.Extensibility
{
    public class DumpLog : IMethod
    {
        public ScriptExecutionResult Execute(ref ScriptExecutionContext context, MethodCallArguments args)
        {
            //setup the log
            string logFilePath = "%bytes.net.dir%\\..\\..\\..\\..\\..\\test\\Scripting\\sampleLog.LOG";
            logFilePath = Helper.ExpandPath(logFilePath);

            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

            //log the data
            context.Log.AddAppender(new RollingFileAppender(logFilePath)); //dumps existing log entries by default

            //return "success"
            return new ScriptExecutionResult(true,"Logged dumped sucessfully to '" + logFilePath + "'");
        }
    }
}
