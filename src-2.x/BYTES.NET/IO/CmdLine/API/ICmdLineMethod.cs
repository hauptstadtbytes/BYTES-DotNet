//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.CmdLine.API
{
    public interface ICmdLineMethod
    {
        string Description { get; }

        CmdLineArgumentDefinition[] Arguments { get; }

        void Execute(ref CmdLineExecutionContext context, CmdLineArguments args);
    }
}
