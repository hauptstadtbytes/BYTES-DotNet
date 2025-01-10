using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Math.Imaging.API
{
    public interface IImage
    {
        /// <summary>
        /// the name property
        /// </summary>
        string Name { get; }

        /// <summary>
        /// method returning a 'BitmapSource' class instance (i.e. for displaying the image in a WPF-based application)
        /// </summary>
        void GetBitmapSource();
    }
}
