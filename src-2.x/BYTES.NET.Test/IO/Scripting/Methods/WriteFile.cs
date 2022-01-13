//import .net namespace(s) required
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO;
using BYTES.NET.IO.Scripting;
using BYTES.NET.Primitives.Extensions;

//import internal namespace(s) required
using BYTES.NET.Test.IO.Scripting.API;

namespace BYTES.NET.Test.IO.Scripting.Methods
{
    public class WriteFile : ITestMethod
    {
        public ExecutionResult Execute(ref TestExecutionContext context, MethodCallArguments args)
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
            return new ExecutionResult(true, "'" + message + "' sucessfully written to '" + logFilePath + "'");
        }
    }
}
