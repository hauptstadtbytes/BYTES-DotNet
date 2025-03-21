using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BYTES.NET.Persistance.API;
using BYTES.NET.Math.Imaging.API;

namespace BYTES.NET.Persistance.Imaging
{
    public interface IImagePersistable : IPersistable
    {
        public void loadImage(IImageParser parser);

        /// <summary>
        /// the name property
        /// </summary>
        string Name { get; }
    }
}
