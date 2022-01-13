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
using BYTES.NET.Primitives.Extensions;

namespace BYTES.NET.Test.IO.Extensibility
{
    public class WriteLog : IMethod
    {
        public ScriptExecutionResult Execute(ref ScriptExecutionContext context, MethodCallArguments args)
        {
            //parse the argument(s)
            string message = "Hello World!";

            if (args.ContainsKey("message"))
            {
                message = args["message"].Expand(context.Variables.Get());
            }

            //setup the log
            string logFilePath = Helper.ExpandPath("%bytes.net.dir%\\..\\..\\..\\..\\..\\test\\Scripting\\sampleLog.LOG");

            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

            //log the data
            File.WriteAllText(logFilePath, message);

            //return "success"
            return new ScriptExecutionResult(true,"'" + message + "' sucessfully written to '" + logFilePath + "'");
        }
    }
}
