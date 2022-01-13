//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.IO.Scripting.API;
using BYTES.NET.IO.Logging;
using BYTES.NET.Primitives.Extensions;
using BYTES.NET.Collections.Extensions;

namespace BYTES.NET.IO.Scripting.Methods
{
    /// <summary>
    /// script method, logging a 'String' type message
    /// </summary>
    /// <remarks>runtime variables masked using the pattern '%name%' will me expanded automatically</remarks>
    [MethodMetdata(Name= "LogMessage", Aliases = new string[] {"Log"})]
    public class LogMessage : IMethod
    {
        public ScriptExecutionResult Execute(ref ScriptExecutionContext context, MethodCallArguments args)
        {
            //validate the argument(s)
            string[]? missingArgs = args.MissingKeys(new string[] { "Message" });

            if (missingArgs.Length > 0)
            {
                return new ScriptExecutionResult(false, "Argument(s) missing", new ArgumentException("Unable to find argument(s) '" + System.String.Join(",", missingArgs) + "'"));
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
            return new ScriptExecutionResult(true, "'LogMessage' executed successfully");
        }
    }
}
