//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BYTES.NET.IO;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Scripting;

//import internal namespace(s) required
using BYTES.NET.Test.IO.Scripting.API;

namespace BYTES.NET.Test.IO.Scripting
{
    public class TestExecutionContext : ExecutionContext<ITestMethod>
    {
        protected override ExecutionResult CallMethod(ITestMethod method, MethodCallArguments args)
        {
            TestExecutionContext me = this;

            return method.Execute(ref me,args);
        }
    }
}
