//import .net namespace(s) reqired
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.IO.Logging;
using BYTES.NET.IO.Scripting.API;

namespace BYTES.NET.IO.Scripting
{
    public class ScriptExecutionContext
    {

        #region protected variable(s)

        protected Log _log = new Log();

        protected Dictionary<string, IMethod> _methods = new Dictionary<string, IMethod>();
        protected ScriptVariables _variables = new ScriptVariables();

        #endregion

        #region public properties

        public Log Log
        {
            get { return _log; }
        }

        public Dictionary<string, IMethod> Methods
        {
            get => _methods; 
            set => _methods = value;
        }

        public ScriptVariables Variables
        {
            get => _variables;
        }

        #endregion

        #region public method(s)

       /// <summary>
       /// executes a 'Script' instance
       /// </summary>
       /// <param name="script"></param>
       /// <returns></returns>
        public ScriptExecutionResult Execute(Script script)
        {
            _log.Inform("Execution of script '" + script.InstanceID + "' started");

            //process the sequence
            ScriptExecutionContext me = this;
            int counter = 0;

            foreach(MethodCall call in script.Sequence)
            {
                counter++;

                if (_methods.ContainsKey(call.MethodID))
                {
                    ScriptExecutionResult result = _methods[call.MethodID].Execute(ref me, call.Arguments);
                    _log.Trace("Processing sequence step " + counter.ToString() + " ('" + call.InstanceID + "') resulted in '" + result.Successful.ToString() + "': " + result.Message, result.Details);

                    if (!result.Successful)
                    {
                        _log.Inform("Execution of script '" + script.InstanceID + "' exited");
                        return new ScriptExecutionResult(false, "Failed to execute sequence step " + counter.ToString() + " ('" + call.InstanceID + "')",result.Details);
                    }
                } else
                {
                    _log.Warn("Processing sequence step " + counter.ToString() + " ('" + call.InstanceID + "') failed: Unable to find method named '" + call.MethodID + "'");
                    _log.Inform("Execution of script '" + script.InstanceID + "' exited");
                    return new ScriptExecutionResult(false,"Unable to find method named '" + call.MethodID + "' for sequence step " + counter.ToString() + " ('" + call.InstanceID + "')");
                }
            }

            //return the default result
            _log.Inform("Execution of script '" + script.InstanceID + "' exited");
            return new ScriptExecutionResult(true, "Script '" + script.InstanceID + "' processed sucessfully");
        }

        #endregion

    }
}
