//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO;
using BYTES.NET.IO.Scripting;
using BYTES.NET.IO.Logging;
using BYTES.NET.Primitives.Extensions;
using BYTES.NET.Collections.Extensions;

//import internal namespace(s) required
using BYTES.NET.Test.IO.Scripting.API;

namespace BYTES.NET.Test.IO.Scripting.Methods
{
    /// <summary>
    /// script method, logging a 'String' type message
    /// </summary>
    /// <remarks>runtime variables masked using the pattern '%name%' will me expanded automatically</remarks>
    [Metdata(Name= "LogMessage", Aliases = new string[] {"Log"})]
    public class LogMessage : ITestMethod
    {
        public ExecutionResult Execute(ref TestExecutionContext context, MethodCallArguments args)
        {
            //validate the argument(s)
            string[]? missingArgs = args.MissingKeys(new string[] { "Message" });

            if (missingArgs.Length > 0)
            {
                return new ExecutionResult(false, "Argument(s) missing", new ArgumentException("Unable to find argument(s) '" + System.String.Join(",", missingArgs) + "'"));
            }

            //parse the information level
            LogEntry.InformationLevel level = LogEntry.InformationLevel.Info;

            if (args.ContainsKey("Level"))
            {
                level = (LogEntry.InformationLevel)Enum.Parse(typeof(LogEntry.InformationLevel), args["Level"].ToString());
            }

            //expand the variable(s)
            string message = args["Message"].Expand(context.Variables.Get());

            //log the message
            context.WriteMessage(message, level);

            //return the (success) output value
            return new ExecutionResult(true, "'LogMessage' executed successfully");
        }
    }
}
