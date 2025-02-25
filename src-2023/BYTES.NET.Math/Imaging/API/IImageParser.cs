using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Math.Imaging.API
{
    /// <summary>
    /// Defines an interface for parsing and loading images.
    /// </summary>
    public interface IImageParser
    {
        /// <summary>
        /// Loads images from the specified disk path.
        /// </summary>
        /// <param name="diskPath">The path to the images on disk.</param>
        /// <returns>An array of IImage instances representing the loaded images.</returns>
        IImage[] Load(string diskPath);
    }
}
