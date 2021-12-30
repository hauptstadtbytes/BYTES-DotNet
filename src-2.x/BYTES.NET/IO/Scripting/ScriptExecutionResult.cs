//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.Scripting
{
    public class ScriptExecutionResult
    {
        #region private variable(s)

        private bool _successful = true;
        private string _message = String.Empty;
        private object? _details = null;

        #endregion

        #region public properties

        public bool Successful
        {
            get => _successful;
        }

        public string Message
        {
            get => _message;
        }

        public object? Details
        {
            get => _details;
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="successful"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public ScriptExecutionResult(bool successful = true, string message = "Finished sucessfully", object? details = null)
        {
            //set the variable(s)
            _successful = successful;
            _message = message;
            _details = details;
        }

        #endregion
    }
}
