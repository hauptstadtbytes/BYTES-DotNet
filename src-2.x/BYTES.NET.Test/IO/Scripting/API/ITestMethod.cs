//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO;
using BYTES.NET.IO.Scripting;

namespace BYTES.NET.Test.IO.Scripting.API
{
    /// <summary>
    /// the interface to be implemented for (scripting) methods
    /// </summary>
    public interface ITestMethod
    {
        ExecutionResult Execute(ref TestExecutionContext context, MethodCallArguments args);
    }
}
