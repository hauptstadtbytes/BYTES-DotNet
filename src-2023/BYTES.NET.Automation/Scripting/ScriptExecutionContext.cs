//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Automation.Scripting
{
    public class ScriptExecutionContext
    {
        #region public method(s)

        /// <summary>
        /// validate the script (before executing)
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public ExecutionResult ValidateScript(Script script)
        {
            //check for the root sequence/ definition
            if (string.IsNullOrEmpty(script.Arguments["RootSequence"]))
            {
                return new ExecutionResult() { Successful = false, Message = "Root sequence missing" };
            }

            if (GetSequence(script, script.Arguments["RootSequence"]) == null)
            {
                return new ExecutionResult() { Successful = false, Message = "No root sequence with ID '" + script.Arguments["RootSequence"] + "' defined" };
            }

            //return the default output
            return new ExecutionResult();
        }

        /// <summary>
        /// executes the root sequence of a script
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public ExecutionResult Execute(Script script)
        {
            //prevalidate the script
            ExecutionResult valResult = ValidateScript(script);

            if (!valResult.Successful)
            {
                return new ExecutionResult() { Successful = false, Message = "Script prevalidation failed: " + valResult.Message };
            }

            //execute the root sequence
            return Execute(script, script.Arguments["RootSequence"]);
        }

        /// <summary>
        /// executes a script sequence, identified by the ID
        /// </summary>
        /// <param name="script"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExecutionResult Execute(Script script, string id)
        {
            //get the sequence
            Sequence? seq = script.Sequences.Find(x => x.ID.ToString().Equals(id));

            if (seq == null)
            {
                return new ExecutionResult() { Successful = false, Message = "Script execusion failed: Unable to find sequence with ID '" + id + "'" };
            }

            //return the default output value
            return new ExecutionResult();
        }

        #endregion

        #region private method(s)

        /// <summary>
        /// searches for a sequence with the ID given inside the script
        /// </summary>
        /// <param name="script"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private Sequence? GetSequence(Script script, string id)
        {
            return script.Sequences.Find(x => x.ID.ToString().Equals(id));
        }

        #endregion
    }
}
