//import .net namespace(s) reqired
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.IO.CmdLine.API;
using BYTES.NET.IO.Logging;
using BYTES.NET.Collections.Extensions;

namespace BYTES.NET.IO.CmdLine
{
    /// <summary>
    /// a generic command line (call) processor
    /// </summary>
    public class CmdLineExecutionContext : ContextBase
    {

        #region protected variable(s)

        protected string _defaultArgPrefix = "-";

        protected Dictionary<string, ICmdLineMethod> _methods = new Dictionary<string, ICmdLineMethod>(StringComparer.OrdinalIgnoreCase);

        protected Func<CmdLineArguments, bool> _checkForGlobalHelpCallback = null;
        protected Action<bool> _returnGlobalHelpCallback = null;

        protected Func<CmdLineArguments, bool> _checkForMethodHelpCallback = null;
        protected Action<string> _returnMethodHelpCallback = null;

        #endregion

        #region public properties

        public string DefaultArgPrefix
        {
            get => _defaultArgPrefix;
            set => _defaultArgPrefix = value;
        }

        public Dictionary<string, ICmdLineMethod> Methods
        {
            get => _methods;
            set => _methods = value;
        }

        public Func<CmdLineArguments, bool> CheckForGlobalHelpCallback
        {
            get => _checkForGlobalHelpCallback;
            set => _checkForGlobalHelpCallback = value;
        }

        public Action<bool> ReturnGlobalHelpCallback
        {
            get => _returnGlobalHelpCallback;
            set => _returnGlobalHelpCallback = value;
        }

        public Func<CmdLineArguments, bool> CheckForMethodHelpCallback
        {
            get => _checkForMethodHelpCallback;
            set => _checkForMethodHelpCallback = value;
        }

        public Action<string> ReturnMethodHelpCallback
        {
            get => _returnMethodHelpCallback;
            set => _returnMethodHelpCallback = value;
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        public CmdLineExecutionContext()
        {
            this.CheckForGlobalHelpCallback = CheckForGlobalHelp;
            this.CheckForMethodHelpCallback = CheckForMethodHelp;

            this.ReturnGlobalHelpCallback = ReturnGlobalHelp;
            this.ReturnMethodHelpCallback = ReturnMethodHelp;
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// executes a call, processing the argument(s) given
        /// </summary>
        /// <param name="args"></param>
        public void Execute(string[] args)
        {
            //parse the argument(s)
            CmdLineArguments arguments = new CmdLineArguments(args);

            //check for a global help request
            if(this.CheckForGlobalHelpCallback(arguments))
            {
                this.ReturnGlobalHelpCallback(true);
                return;
            }

            //check for a method-specific help request
            if (this.CheckForMethodHelpCallback(arguments))
            {
                this.ReturnMethodHelpCallback(arguments.Raw[0]);
                return;
            }

            //validates the argument(s) given
            if (!this.Methods.ContainsKey(arguments.Raw[0])) //the method is unknown
            {
                ReportError("Method '" + arguments.Raw[0] + "' unknown. Unable to proceed.");

                WriteMessage(string.Empty);
                this.ReturnGlobalHelpCallback(false);

                return;
            }

            ICmdLineMethod method = this.Methods[arguments.Raw[0]];
            string[] compulsatoryArgs = GetCompulsatoryArgs(method);

            if(compulsatoryArgs.Length > 0)
            {

                string[] missing = arguments.MissingKeys(compulsatoryArgs);

                if(missing.Length > 0)
                {
                    ReportError("Argument(s) '" + String.Join("','",missing) + "' missing for method '" + arguments.Raw[0] + "'. Unable to proceed.");

                    WriteMessage(string.Empty);
                    this.ReturnMethodHelpCallback(arguments.Raw[0]);

                    return;
                }
            }

            //execute the method requested
            CmdLineExecutionContext me = this;
            method.Execute(ref me, arguments);
        }

        #endregion

        #region protected method(s)

        /// <summary>
        /// checks for (global) help requests
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual bool CheckForGlobalHelp(CmdLineArguments args)
        {
            if(args.Raw.Length < 1 || args.IsNamedArgument(0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// returns (global) help
        /// </summary>
        /// <param name="showGeneralRemarks"></param>
        protected virtual void ReturnGlobalHelp(bool showGeneralRemarks)
        {
            if (showGeneralRemarks)
            {
                WriteMessage("Method argument missing. Unable to proceed.");
                WriteMessage("Please use the following pattern: <method> [arguments] [options]");
                WriteMessage("For details on a specific method(s), use the pattern: <method> /?");
            }

            WriteMessage(String.Empty);
            WriteMessage("Available method(s):");
            foreach (KeyValuePair<String, ICmdLineMethod> pair in Methods)
            {
                WriteMessage(pair.Key + " - " + pair.Value.Description);
            }
        }

        /// <summary>
        /// checks for method-specific help requests
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected virtual bool CheckForMethodHelp(CmdLineArguments args)
        {
            if(!args.IsNamedArgument(0) && (args.ContainsKey("?") | args.ContainsKey("help"))){
                return true;
            }

            return false;
        }

        /// <summary>
        /// returns method-specific help
        /// </summary>
        /// <param name="name"></param>
        protected virtual void ReturnMethodHelp(string name)
        {
            //get the method details
            ICmdLineMethod method = this.Methods[name];

            //enumerate arguments by type
            Dictionary<string, List<CmdLineArgumentDefinition>> args = AggregateArgsDefinitions(method);

            //write the output
            WriteMessage("Method: " + name);
            WriteMessage("Description: " + method.Description);

            string usageMsg = "Usage: " + name;

            if(args["compulsatory"].Count > 0)
            {
                usageMsg += " [arguments]";
            }

            if (args["optional"].Count > 0)
            {
                usageMsg += " [options]";
            }

            WriteMessage(usageMsg);

            if (args["compulsatory"].Count > 0)
            {
                WriteMessage(String.Empty);
                WriteMessage("Available argument(s):");
                foreach (CmdLineArgumentDefinition definition in args["compulsatory"])
                {
                    WriteMessage("[" + this.DefaultArgPrefix + definition.Name + "] " + definition.Description);
                }
            }

            if (args["optional"].Count > 0)
            {
                WriteMessage(String.Empty);
                WriteMessage("Available option(s):");
                foreach (CmdLineArgumentDefinition definition in args["optional"])
                {
                    WriteMessage("[" + this.DefaultArgPrefix + definition.Name + "] " + definition.Description);
                }
            }
        }

        /// <summary>
        /// returns a dictionary of aggregated arguments definitions
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        protected Dictionary<string, List<CmdLineArgumentDefinition>> AggregateArgsDefinitions(ICmdLineMethod method)
        {
            Dictionary<string, List<CmdLineArgumentDefinition>> output = new Dictionary<string, List<CmdLineArgumentDefinition>>(StringComparer.OrdinalIgnoreCase) { {"compulsatory", new List<CmdLineArgumentDefinition>()}, { "optional", new List<CmdLineArgumentDefinition>() } };

            foreach (CmdLineArgumentDefinition definition in method.Arguments)
            {
                if (definition.IsCompulsory)
                {
                    output["compulsatory"].Add(definition);
                }
                else
                {
                    output["optional"].Add(definition);
                }
            }

            return output;
        }

        /// <summary>
        /// returns a list of compulsatory arguments for the given method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        protected string[] GetCompulsatoryArgs(ICmdLineMethod method)
        {
            List<string> output = new List<string>();

            Dictionary<string, List<CmdLineArgumentDefinition>> aggregated = AggregateArgsDefinitions(method);

            foreach(CmdLineArgumentDefinition def in aggregated["compulsatory"])
            {
                output.Add(def.Name);
            }

            return output.ToArray();
        }

        #endregion

    }
}
