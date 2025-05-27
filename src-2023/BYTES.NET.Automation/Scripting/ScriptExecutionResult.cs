//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Automation.Scripting
{
    public class ScriptExecutionResult : ExecutionResult
    {
        #region (additional) public properties

        public Guid Sequence { get; set; }

        public int Step { get; set; }

        #endregion
    }
}
