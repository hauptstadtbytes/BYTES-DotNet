//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Collections.Extensions;
using BYTES.NET.IO;

//import internal namespace(s) required
using BYTES.NET.Test.IO.Scripting.API;
using BYTES.NET.IO.Scripting;

namespace BYTES.NET.Test.IO.Scripting.Methods
{
    /// <summary>
    /// script method, setting a runtime variable
    /// </summary>
    [Metdata(Name = "SetVariable", Aliases = new string[] {"Set"})]
    public class SetVariable : ITestMethod
    {
        public ExecutionResult Execute(ref TestExecutionContext context, MethodCallArguments args)
        {
            //validate the argument(s)
            string[]? missingArgs = args.MissingKeys(new string[] { "Name", "Value" });

            if (missingArgs.Length > 0)
            {
                return new ExecutionResult(false, "Argument(s) '" + System.String.Join(",", missingArgs) + "' missing");
            }

            //set the variable
            context.Variables.Set(args["name"], args["Value"]);

            //return the (success) output value
            return new ExecutionResult(true, "'SetVariable' executed successfully");
        }
    }
}
