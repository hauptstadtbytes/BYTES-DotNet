//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.Extensibility.API
{
    public interface IExtensionsSource
    {
        string Source { get; set; }
        string Pattern { get; set; }

        /// <summary>
        /// returns the list of source assembly paths
        /// </summary>
        /// <returns></returns>
        string[] AssemblyPaths(Dictionary<string, string> variables = null);

        /// <summary>
        /// returns all valid extensions found
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="variables"></param>
        /// <returns></returns>
        Extension<TInterface>[] Extensions<TInterface>(Dictionary<string, string> variables = null);
        Extension<TInterface, TMetadata>[] Extensions<TInterface, TMetadata>(Dictionary<string, string> variables = null);

        /// <summary>
        /// validates the metadata of a (potential) extension
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TMetadata"></typeparam>
        /// <param name="extension"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        bool ValidateExtensionMetadata<TInterface, TMetadata>(Lazy<TInterface, TMetadata> extension, string pattern = null);
    }
}
