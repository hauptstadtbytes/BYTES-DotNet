//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.Scripting.API
{
    /// <summary>
    /// the interface to be implemented for (scripting) methods
    /// </summary>
    public interface IMethod
    {
        ScriptExecutionResult Execute(ref ScriptExecutionContext context, MethodCallArguments args);
    }
}
