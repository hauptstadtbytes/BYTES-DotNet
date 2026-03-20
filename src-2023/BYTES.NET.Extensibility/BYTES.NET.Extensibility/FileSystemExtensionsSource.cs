//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

//import internal namespace(s) required
using BYTES.NET.Extensibility.API;
using BYTES.NET.Logging;
using BYTES.NET.IO;
using BYTES.NET.Primitives;
using System.Text.RegularExpressions;
using System.Reflection;

namespace BYTES.NET.Extensibility
{
    /// <summary>
    /// the (default) file system extensions source class
    /// </summary>
    [Serializable]
    [XmlType("Source")]
    public class FileSystemExtensionsSource : IExtensionsSource
    {
        #region private variable(s)

        private string _source = System.String.Empty;
        private string _pattern = "*";

        private Log _log = new Log() { CacheLimit = -1 };

        #endregion

        #region public properties, supporting 'IExtensionsSource'

        [XmlAttribute]
        public string Source { get => _source; set => _source = value; }

        [XmlAttribute]
        public string Pattern { get => _pattern; set => _pattern = value; }

        [XmlIgnore]
        public Log Log { get => _log; }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <remarks>required for XML serialization</remarks>
        public FileSystemExtensionsSource()
        {
        }

        /// <summary>
        /// new instance method, supporting source (path) and pattern definition
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        public FileSystemExtensionsSource(string source, string pattern = null)
        {
            _source = source;

            if (pattern != null)
            {
                _pattern = pattern;
            }
        }

        #endregion

        #region public method(s), supporting 'IExtensionsSource'

        /// <summary>
        /// returns a list of assembly paths, proably containing extension(s)
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        public string[] AssemblyPaths(Dictionary<string, string> variables = null)
        {
            //return empty list for empty source path definition
            if (Source == null)
            {
                return new string[] { };
            }
            else if (System.String.IsNullOrEmpty(Source) || System.String.IsNullOrWhiteSpace(Source))
            {
                return new string[] { };
            }

            //split by '|' (if existing)
            string[] parsedPaths = null;
            if (Source.Contains("|"))
            {
                parsedPaths = Source.Split('|');
            }
            else
            {
                parsedPaths = new string[] { Source };
            }

            //expand the path(s)
            if (variables == null)
            {
                variables = new Dictionary<string, string>();
            }

            //get the assembly paths
            List<string> output = new List<string>();

            foreach (string path in parsedPaths)
            {
                foreach (string assembly in path.ExpandWildcardPath(variables))
                {
                    output.Add(assembly);
                }
            }

            //return the default output
            return output.ToArray();
        }

        /// <summary>
        /// return alls (valid) extensions found
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="variables"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Extension<TInterface>[] Extensions<TInterface>(Dictionary<string, string> variables = null)
        {
            //reset the log
            _log = new Log() { CacheLimit = -1, Threshold = LogEntry.InformationLevel.Debug };

            //enumerate the extensions
            List<Extension<TInterface>> output = new List<Extension<TInterface>>();

            foreach (string path in AssemblyPaths(variables)) //loop for each assembly path parsed
            {

                try //try to load the assembly file and iterate over the types
                {

                    foreach (System.Type type in GetImplementations<TInterface>(path))
                    {

                        try
                        {
                            output.Add(new Extension<TInterface>(type, () => (TInterface)Activator.CreateInstance(type))); //add a lazy-based 'Extension' instance
                        }
                        catch (Exception ex)
                        {
                            _log.Warn("Failed to create lazy instance of '" + type.ToString() + "' at '" + path + "'", ex);
                        }

                    }

                }
                catch (Exception ex)
                {
                    _log.ReportError("Failed to load extensions from '" + path + "'", ex);
                }

            }

            //return the output
            return output.ToArray();
        }

        /// <summary>
        /// returns all (valid) extensions found, supporting metadata
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TMetadata"></typeparam>
        /// <param name="variables"></param>
        /// <returns></returns>
        public Extension<TInterface, TMetadata>[] Extensions<TInterface, TMetadata>(Dictionary<string, string> variables = null)
        {
            //reset the log
            _log = new Log() { CacheLimit = -1, Threshold = LogEntry.InformationLevel.Debug };

            //enumerate the extension(s)
            List<Extension<TInterface, TMetadata>> output = new List<Extension<TInterface, TMetadata>>();

            foreach (string path in AssemblyPaths(variables))
            {

                try
                {

                    foreach (System.Type type in GetImplementations<TInterface>(path))
                    {

                        if (Attribute.GetCustomAttribute(type, typeof(TMetadata)) != null) //check for an existing metadata instance
                        {

                            try
                            {
                                Extension<TInterface, TMetadata> instance = new Extension<TInterface, TMetadata>(type, () => (TInterface)Activator.CreateInstance(type), (TMetadata)Convert.ChangeType(Attribute.GetCustomAttribute(type, typeof(TMetadata)), typeof(TMetadata))); //add a lazy-based 'Extension' instance

                                if (this.ValidateExtensionMetadata<TInterface, TMetadata>(instance, _pattern)) //validate the metadata (value(s))
                                {
                                    output.Add(instance);
                                }

                            }
                            catch (Exception ex)
                            {
                                _log.Warn("Failed to create lazy instance of '" + type.ToString() + "' at '" + path + "'", ex);
                            }

                        }

                    }

                }
                catch (Exception ex)
                {
                    _log.Warn("Failed to load extensions from '" + path + "'", ex);
                }

            }

            return output.ToArray();
        }

        /// <summary>
        /// validates the metadata of a (potential) extension
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TMetadata"></typeparam>
        /// <param name="extension"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ValidateExtensionMetadata<TInterface, TMetadata>(Lazy<TInterface, TMetadata> extension, string? pattern = null)
        {
            //parse the argument(s)
            if (pattern == null)
            {
                pattern = _pattern;
            }

            //return always 'true' for wildcards
            if (pattern == null)
            {
                _log.Trace("Metadata validation: Wildcard found");
                return true;
            }
            else if (pattern == "*" || System.String.IsNullOrEmpty(pattern) || System.String.IsNullOrWhiteSpace(pattern))
            {
                _log.Trace("Metadata validation: Wildcard found");
                return true;
            }

            //parse the pattern
            string[] patterns = new string[] { pattern };

            if (pattern.Contains("|"))
            {
                patterns = pattern.Split('|');
            }

            //validate each single pattern
            foreach (string pat in patterns)
            {

                //return 'true' for wildcard
                if (pat == "*")
                {
                    _log.Trace("Metadata validation: Wildcard found");
                    return true;
                }

                try
                {

                    //parse the definition
                    KeyValuePair<string, string> definition = pat.ParseKeyValue();
                    _log.Trace("Metadata validation: Filter definition [" + definition.Key + "," + definition.Value + "]");

                    string? value = null;
                    try
                    {
                        value = extension.Metadata.GetType().GetProperty(definition.Key).GetValue(extension.Metadata, null).ToString();

                    }
                    catch (Exception ex)
                    {
                        _log.Trace("Failed to validate metdata: Unable to read property named '" + definition.Key + "'", ex);
                        break;
                    }
                    _log.Trace("Metadata validation: Property value [" + value + "]");

                    //compare (instance) value with definition
                    if (definition.Value.Contains("*"))
                    {

                        string regexString = definition.Value.Replace("*", @"[\w|-]");
                        _log.Trace("Metadata validation: Matching '" + value + "' with '" + regexString + "' using regular expression");

                        Regex myRegex = new Regex(regexString, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                        foreach (Match theMatch in myRegex.Matches(value))
                        {
                            if (theMatch.Success)
                            {
                                return true;
                            }
                        }

                    }
                    else
                    {

                        _log.Trace("Metadata validation: Matching '" + value + "' with '" + definition.Value + "' by string comparing");
                        if (definition.Value.ToLower() == value.ToLower())
                        {
                            return true;
                        }

                    }

                }
                catch (Exception ex)
                {
                    _log.ReportError("Failed to validate metadata: " + ex.Message, ex);
                }

            }

            //return the default output
            return false;
        }

        #endregion

        #region private method(s)

        /// <summary>
        /// returns all implementation(s)/ sub-classes of a given class type or interface
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="path"></param>
        /// <param name="validateForActivator"></param>
        /// <returns></returns>
        private System.Type[] GetImplementations<TInterface>(string path, bool validateForActivator = true)
        {
            List<System.Type> output = new List<System.Type>();

            //load the assembly
            Assembly file = Assembly.LoadFrom(path);

            //enumerate the valid types
            foreach (System.Type type in file.GetTypes())
            {

                if (typeof(TInterface).GetTypeInfo().IsInterface) //get interface implementation(s) only
                {

                    if (type.GetInterfaces().Contains(typeof(TInterface)))
                    {
                        if (ValidateForActivator(type.GetTypeInfo(), validateForActivator))
                        {
                            output.Add(type);
                        }
                    }

                }

                if (typeof(TInterface).GetTypeInfo().IsClass) //get all sub-classes of a given type
                {

                    if (type.IsSubclassOf(typeof(TInterface)))
                    {
                        if (ValidateForActivator(type.GetTypeInfo(), validateForActivator))
                        {
                            output.Add(type);
                        }
                    }

                }

            }

            //return the output
            return output.ToArray();
        }

        /// <summary>
        /// validates a given type for using the activator
        /// </summary>
        /// <param name="info"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        private bool ValidateForActivator(TypeInfo info, bool validate = true)
        {

            if (!validate)
            {
                return true;
            }

            if (info.IsClass && !info.IsAbstract)
            {
                return true;
            }

            return false;

        }

        #endregion
    }
}
