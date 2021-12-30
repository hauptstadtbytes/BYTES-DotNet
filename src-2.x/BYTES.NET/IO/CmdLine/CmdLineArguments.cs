//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Primitives.Extensions;
using BYTES.NET.Collections;

namespace BYTES.NET.IO.CmdLine
{
    /// <summary>
    /// a command line arguments collection
    /// </summary>
    public class CmdLineArguments : ExtendedDictionary<string,string>
    {

        #region private variable(s)

        private string[] _args = { };
        private Regex _namedArgsRegEx = new Regex(@"[-|/]([\w|'|""|?]*)[:|=]*([\w|'|""|:|\\|\s|!]*)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

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

        public string[] Raw
        {
            get => _args;
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="args"></param>
        public CmdLineArguments(string[] args) : base(StringComparer.OrdinalIgnoreCase)
        {
            _args = args;
            ParseNamedArgs();
        }

        #endregion

        #region public method(s)

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
        private void ParseNamedArgs()
        {

            int counter = -1;

            foreach(string arg in _args)
            {

                counter += 1;

                Match? match;

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

                    this.Add(key,value.Trim('"').Trim('\''));
                }
            }

        }

        #endregion

    }
}
