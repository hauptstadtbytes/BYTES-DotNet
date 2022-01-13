//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.IO.Scripting.API;
using BYTES.NET.Collections.Extensions;

namespace BYTES.NET.IO.Scripting.Methods
{
    /// <summary>
    /// script method, setting a runtime variable
    /// </summary>
    [MethodMetdata(Name = "SetVariable", Aliases = new string[] {"Set"})]
    public class SetVariable : IMethod
    {
        public ScriptExecutionResult Execute(ref ScriptExecutionContext context, MethodCallArguments args)
        {
            //validate the argument(s)
            string[]? missingArgs = args.MissingKeys(new string[] { "Name", "Value" });

            if (missingArgs.Length > 0)
            {
                return new ScriptExecutionResult(false, "Argument(s) '" + String.Join(",", missingArgs) + "' missing");
            }

            //set the variable
            context.Variables.Set(args["name"],args["Value"]);

            //return the (success) output value
            return new ScriptExecutionResult(true, "'SetVariable' executed successfully");
        }

    }
}
