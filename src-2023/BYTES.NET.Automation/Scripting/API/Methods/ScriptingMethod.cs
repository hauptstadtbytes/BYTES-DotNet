//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Automation.Scripting.API;

namespace BYTES.NET.Automation.Scripting.API.Methods
{
    public abstract class ScriptingMethod : IScriptingMethod
    {
        //execute the task
        public abstract ExecutionResult Execute(ScriptingEntityArguments args, ScriptExecutionContext context); 
    }
}
