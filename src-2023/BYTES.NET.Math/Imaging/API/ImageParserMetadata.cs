using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Math.Imaging.API
{
    public class ImageParserMetadata
    {
        #region Public Properties
        public string Name;
        public string FileExtension;
        #endregion

        #region public new instance method(s)
        public ImageParserMetadata(string name, string fileExtension)
        {
            Name = name;
            FileExtension = fileExtension;
        }
        #endregion
    }
}
