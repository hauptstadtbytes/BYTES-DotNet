using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Math.Imaging.API
{
    /// <summary>
    /// Defines metadata for an image parser, extending the IMetadata interface.
    /// </summary>
    public interface IImageParserMetadata
    {
        /// <summary>
        /// Gets the name of the image parser.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the supported file extensions for the image parser.
        /// </summary>
        string[] FileExtensions { get; }
    }
}
