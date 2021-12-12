//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Primitives.Extensions;

namespace BYTES.NET.Collections
{
    /// <summary>
    /// a collection of (command line) arguments
    /// </summary>
    public class ArgumentsCollection
    {

        #region private variable(s)

        private string[] _args = { };

        private Regex _namedArgsRegEx = new Regex(@"[-|/]([\w|'|""]*)[:|=]*([\w|'|""|:|\\]*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private Dictionary<string, string> _namedArgs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region public properties

        public string this[int index]
        {
            get
            {
                if(index > 0 || index > _args.Length - 1)
                {
                    throw new ArgumentException("Unable to find argument with index '" + index + "' in collection with '" + _args.Length + "' item(s)");
                }

                return _args[index];
            }
        }

        public string this[string name]
        {
            get
            {
                if (!_namedArgs.ContainsKey(name))
                {
                    throw new ArgumentException("Unable to find argument named '" + name + "'");
                }

                return _namedArgs[name];
            }
        }

        public string[] Arguments
        {
            get => _args;
        }

        public string[] Names
        {
            get => _namedArgs.Keys.ToArray<string>();
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="args"></param>
        public ArgumentsCollection(string[] args)
        {
            _args = args;
            _namedArgs = ParseNamedArgs();
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// checks for a dedicated named argument
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsArgument(string name)
        {
            return _namedArgs.ContainsKey(name);
        }

        /// <summary>
        /// checks an indexed argument for being a named one
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsNamedArgument(int index)
        {

            if (this[index].MatchesPattern(_namedArgsRegEx))
            {
                return true;
            }

            return false;

        }

        #endregion

        #region private method(s)

        /// <summary>
        /// parses the named argument(s)
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> ParseNamedArgs()
        {
            Dictionary<string,string> output = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);

            int counter = -1;

            foreach(string arg in _args)
            {

                counter += 1;

                Match ?match;

                if (arg.MatchesPattern(_namedArgsRegEx,out match)) //check for a valid argument, extracting key (and value)
                {
                    string key = match.Groups[1].Value.ToString();
                    string value = string.Empty;

                    if (match != null)
                    {
                        value = match.Groups[2].Value.ToString();
                    }

                    if (string.IsNullOrEmpty(value))
                    {
                        if(_args.Length >= counter + 2)
                        {
                            if (!_args[counter + 1].MatchesPattern(_namedArgsRegEx)) //check for following argument
                            {
                                value = _args[counter + 1];
                            }
                        }
                    }

                    output.Add(key,value.Trim('"').Trim('\''));
                }
            }

            return output;
        }

        #endregion

    }
}
