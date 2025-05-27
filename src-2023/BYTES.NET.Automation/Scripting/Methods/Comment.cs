//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) from 'BYTES.NET.Automation' frameworks
using BYTES.NET.Automation.Scripting.API.Methods;

namespace BYTES.NET.Automation.Scripting.Methods
{
    [ScriptingMethodMetadata(Name = "Comment", Description = "a simple script comment, not resulting in any action")]
    public class Comment : ScriptingMethod
    {

        //execute the method call
        public override ExecutionResult Execute(ScriptingEntityArguments args, ScriptExecutionContext context)
        {
            //do nothing

            //return the output value
            return new ExecutionResult();
        }

    }
}
