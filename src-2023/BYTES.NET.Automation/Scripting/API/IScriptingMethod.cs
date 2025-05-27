//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required

namespace BYTES.NET.Automation.Scripting.API
{
    internal interface IScriptingMethod
    {
        //execute the method
        ExecutionResult Execute(ScriptingEntityArguments args, ScriptExecutionContext context);
    }
}
