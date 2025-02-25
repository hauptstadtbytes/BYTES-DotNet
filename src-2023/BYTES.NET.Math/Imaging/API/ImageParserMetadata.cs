using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Math.Imaging.API
{
    /// <summary>
    /// Represents metadata for an image parser, extending the Metadata class.
    /// </summary>
    public class ImageParserMetadata
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the image parser.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the supported file extensions for the image parser.
        /// </summary>
        public string[] FileExtensions { get; set; }

        #endregion

        #region Public New Instance Method(s)

        /// <summary>
        /// Default constructor for ImageParserMetadata.
        /// </summary>
        public ImageParserMetadata()
        {
        }

        #endregion
    }
}
