//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Automation
{
    public class ExecutionResult
    {
        #region public properties

        public bool Successful {  get; set; }
        public string Message { get; set; }

        #endregion

        #region public new instance method(s)

        public ExecutionResult() {

            this.Successful = true;
        }

        #endregion
    }
}
