//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Extensibility.API;
using BYTES.NET.Logging;

namespace BYTES.NET.Extensibility
{
    /// <summary>
    /// the (basic) extensions manager class
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    public class ExtensionsManager<TInterface>
    {
        #region protected variable(s)

        protected Log _log = new Log() { CacheLimit = -1, Threshold = LogEntry.InformationLevel.Debug };
        protected Extension<TInterface>[] _extensions = new Extension<TInterface>[] { };

        protected Dictionary<string, string> _variables = new Dictionary<string, string>();
        protected IExtensionsSource[] _sources = new IExtensionsSource[] { };

        #endregion

        #region public properties

        public Extension<TInterface>[] Extensions { get => _extensions; }

        public Log Log { get => _log; }

        public Dictionary<string, string> Variables { get => _variables; set { _variables = value; } }

        public IExtensionsSource[] Sources { get => _sources; }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="paths"></param>
        public ExtensionsManager(string[]? paths = null)
        {
            if (paths != null)
            {
                Update(paths);
            }
        }

        /// <summary>
        /// new instance method, supporting 'ExtensionsSource' type definition
        /// </summary>
        /// <param name="sources"></param>
        public ExtensionsManager(IExtensionsSource[] sources)
        {
            Update(sources);
        }

        #endregion

        #region  public method(s)

        /// <summary>
        /// (re-)indexes the extension(s) (from 'IExtensionsSource' list given)
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="resetLog"></param>
        public void Update(IExtensionsSource[]? sources = null, bool resetLog = true)
        {

            //reset the log (if requested)
            if (resetLog)
            {
                _log = new Log() { CacheLimit = -1 };
            }

            //parse the argument(s)
            if(sources != null)
            {
                _sources = sources;
            }

            if(_sources.Length < 1)
            {
                _log.Trace("No extension source(s) configured");
            }

            //get the extension(s)
            List<Extension<TInterface>> extensions = new List<Extension<TInterface>>();

            foreach (IExtensionsSource source in _sources) //loop for each source
            {
                Lazy<TInterface>[] foundExtensions = source.Extensions<TInterface>(_variables);
                _log.Trace(foundExtensions.Length.ToString() + " extensions found from source '" + source.Source + "' using pattern '" + source.Pattern + "'");

                foreach (Extension<TInterface> extension in foundExtensions) //loop for each extension found
                {
                    extensions.Add(extension);
                }
            }

            _extensions = extensions.ToArray();
            _log.Trace(_extensions.Length.ToString() + " extensions found from " + _sources.Length.ToString() + " sources");

        }

        /// <summary>
        /// method, (re-)indexinng the extensions, supporting filesystem path(s)
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="resetLog"></param>
        public void Update(string[] paths, bool resetLog = true)
        {
            List<IExtensionsSource> sources = new List<IExtensionsSource>();

            foreach (string path in paths)
            {
               sources.Add(new FileSystemExtensionsSource() { Source = path });
            }

            Update(sources.ToArray(), resetLog);
        }

        #endregion
    }

    /// <summary>
    /// the extensions manager class supporting metadata
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TMetadata"></typeparam>
    public class ExtensionsManager<TInterface, TMetadata> : ExtensionsManager<TInterface>
    {

        #region protected variable(s)

        new protected Extension<TInterface, TMetadata>[] _extensions = new Extension<TInterface, TMetadata>[] { };

        #endregion

        #region public properties

        new public Extension<TInterface, TMetadata>[] Extensions { get => _extensions; }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="paths"></param>
        public ExtensionsManager(string[]? paths = null) : base(paths)
        {
            if (paths != null)
            {
                Update(paths);
            }
        }

        /// <summary>
        /// new instance method, supporting 'ExtensionsSource' type definition
        /// </summary>
        /// <param name="sources"></param>
        public ExtensionsManager(IExtensionsSource[] sources) : base(sources)
        {
            Update(sources);
        }

        #endregion

        #region  public method(s)

        /// <summary>
        /// (re-)indexes the extension(s) (from 'IExtensionsSource' list given)
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="resetLog"></param>
        new public void Update(IExtensionsSource[]? sources = null, bool resetLog = true)
        {

            //reset the log (if requested)
            if (resetLog)
            {
                _log = new Log() { CacheLimit = -1 };
            }

            //parse the argument(s)
            if (sources != null)
            {
                _sources = sources;
            }

            if (_sources.Length < 1)
            {
                _log.Trace("No extension source(s) configured");
            }

            //get the extension(s)
            List<Extension<TInterface, TMetadata>> extensions = new List<Extension<TInterface, TMetadata>>();

            foreach (IExtensionsSource source in _sources) //loop for each source
            {
                Lazy<TInterface, TMetadata>[] foundExtensions = source.Extensions<TInterface, TMetadata>(_variables);
                _log.Trace(foundExtensions.Length.ToString() + " extensions found from source '" + source.Source + "' using pattern '" + source.Pattern + "'");

                foreach (Extension<TInterface, TMetadata> extension in foundExtensions) //loop for each extension found
                {
                    extensions.Add(extension);
                }
            }

            _extensions = extensions.ToArray();
            _log.Trace(_extensions.Length.ToString() + " extensions found from " + _sources.Length.ToString() + " sources");

        }

        /// <summary>
        /// overloaded method, (re-)loads the extensions (from string paths list)
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="resetLog"></param>
        new public void Update(string[] paths, bool resetLog = true)
        {
            List<IExtensionsSource> sources = new List<IExtensionsSource>();

            foreach (string path in paths)
            {
              sources.Add(new FileSystemExtensionsSource() { Source = path });
            }

            Update(sources.ToArray(), resetLog);
        }

        #endregion
    }

}
