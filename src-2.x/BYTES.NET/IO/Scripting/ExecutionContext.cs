//import .net namespace(s) reqired
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.Scripting
{
    public abstract class ExecutionContext<TInterface> : ContextBase
    {

        #region protected variable(s)

        protected Dictionary<string, TInterface> _methods = new Dictionary<string, TInterface>();
        protected ExecutionContextVariables _variables = new ExecutionContextVariables();

        #endregion

        #region public properties

        public Dictionary<string, TInterface> Methods
        {
            get => _methods; 
            set => _methods = value;
        }

        public ExecutionContextVariables Variables
        {
            get => _variables;
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// executes a sequence
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public ExecutionResult Execute(Sequence sequence)
        {
            //parse the sequence properties
            string seqName = "'" + sequence.ID + "'";

            if (String.IsNullOrEmpty(sequence.Name))
            {
                seqName = "'" + sequence.Name + "' (" + seqName + ")";
            }

            Trace("Sequence " + seqName + " execution started");

            //process the sequence
            int counter = 0;

            foreach(MethodCall call in sequence.Calls)
            {
                counter++;

                //parse the step properties
                string stepName = "'" + counter.ToString() + "' (" + call.ID + ")";

                if (_methods.ContainsKey(call.Method))
                {
                    ExecutionResult result = CallMethod(_methods[call.Method], call.Arguments);
                    Trace("Processing step " + stepName + " of sequence " + seqName + " resulted in '" + result.Successful.ToString() + "': " + result.Message, result.Details);

                    if (!result.Successful)
                    {
                        Inform("Sequence " + seqName + " aborted: Step " + stepName + " failed");
                        return new ExecutionResult(false, "Failed to execute step " + stepName + " in sequence " + seqName,result.Details);
                    }
                } else
                {
                    Trace("Sequence " + seqName + " aborted: Unable to find method named '" + call.Method + "' required for step " + stepName);
                    return new ExecutionResult(false, "Unable to find method named '" + call.Method + "' required for step " + stepName + " in sequence " + seqName);
                }
            }

            //return the default result
            Trace("Sequence " + seqName + " execution finished");
            return new ExecutionResult(true, "Sequence " + seqName + " processed sucessfully");
        }

        #endregion

        #region protected method(s)

        /// <summary>
        /// performs a (single) method call
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected abstract ExecutionResult CallMethod(TInterface method, MethodCallArguments args);

        #endregion
    }
}
