//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Extensibility;
using BYTES.NET.Logging;

//import internal namespace(s) required
using BYTES.NET.Automation.Scripting.API;
using BYTES.NET.Automation.Scripting.API.Methods;

namespace BYTES.NET.Automation.Scripting
{
    public class ScriptExecutionContext
    {
        #region private variable(s)

        private string[] _searchPaths = new string[] { "%BYTES.NET.DIR%\\*.dll" };

        private ExtensionsManager<IScriptingMethod, ScriptingMethodMetadata> _methodsManager = new ExtensionsManager<IScriptingMethod, ScriptingMethodMetadata>();

        #endregion

        #region "public properties"

        public string[] SearchPaths { 

            get => _searchPaths; 
            set { 
                _searchPaths = value;
                _methodsManager.Update(_searchPaths);
            } 

        }

        public Log Log { get; set; }

        #endregion

        #region public new instance method(s)

        //default constructor
        public ScriptExecutionContext()
        {

            Log = new Log(); //create a new log instance

            _methodsManager.Update(SearchPaths); //update the extensions manager

        }

        #endregion

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
        public ScriptExecutionResult Execute(Script script)
        {
            //prevalidate the script
            ExecutionResult valResult = ValidateScript(script);

            if (!valResult.Successful)
            {
                return new ScriptExecutionResult() { Successful = false, Message = "Script prevalidation failed: " + valResult.Message };
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
        public ScriptExecutionResult Execute(Script script, string id)
        {
            //get the sequence
            Sequence? seq = script.Sequences.Find(x => x.ID.ToString().Equals(id));

            if (seq == null)
            {
                return new ScriptExecutionResult() { Successful = false, Message = "Script execusion failed: Unable to find sequence with ID '" + id + "'" };
            }

            //loop for each method call
            int counter = 0;

            foreach(MethodCall call in seq.Calls)
            {

                counter++;

                //try to get the method implementation
                Extension<IScriptingMethod, ScriptingMethodMetadata> extension = GetExtension(call.Method);

                if(extension == null)
                {
                    return new ScriptExecutionResult() { Successful = false, Message = "Unable to find method '" + call.Method + "'", Step = counter, Sequence = seq.ID };
                }

                //execute the method
                ExecutionResult result = extension.Value().Execute(call.Arguments,this);

                this.Log.Trace("Method '" + call.Method + "' finished with message '" + result.Message + "'");

                if(result.Successful != true)
                {
                    return new ScriptExecutionResult() { Successful = false, Message = "Method '" + call.Method + "' failed with message '" + result.Message + "'", Step = counter, Sequence = seq.ID };
                }

            }

            //return the default output value
            return new ScriptExecutionResult();
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

        /// <summary>
        /// returns the method extension, identified by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Extension<IScriptingMethod,ScriptingMethodMetadata>? GetExtension(string methodName)
        {

            foreach(Extension<IScriptingMethod,ScriptingMethodMetadata> extension in _methodsManager.Extensions)
            {
                if(methodName.ToLower() == extension.Metadata.Name.ToLower())
                {
                    return extension;
                }
                
            }

            return null;

        }

        #endregion
    }

}
