using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BYTES.NET.Math.Imaging.API;
using BYTES.NET.Persistance.API;

namespace BYTES.NET.Math.Imaging
{
    public class RGBImage : IImage
    {
        #region Private Fields

        private string _name = Guid.NewGuid().ToString();
        private GraylevelImage _red;
        private GraylevelImage _green;
        private GraylevelImage _blue;

        #endregion

        #region Public Properties Implementing IImage

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        #endregion

        #region Constructors

        public RGBImage(ref GraylevelImage red, ref GraylevelImage green, ref GraylevelImage blue)
        {
            _red = red;
            _green = green;
            _blue = blue;
        }
        #endregion

        #region Public Methods Implementing IImage

        public byte[] GetRawImageData(out int width, out int height, out int stride)
        {
            width = _red.Width;
            height = _red.Height;

            stride = width * 3;
            int numPaddingBytes = 0;

            while (stride % 4 != 0) // Ensure 4-byte alignment
            {
                stride++;
                numPaddingBytes++;
            }

            byte[] bits = new byte[stride * height];

            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    int pos = row * stride + col * 3;
                    bits[pos] = (byte)_red.Values[col + 1, row + 1];   // Red
                    bits[pos + 1] = (byte)_green.Values[col + 1, row + 1]; // Green
                    bits[pos + 2] = (byte)_blue.Values[col + 1, row + 1];  // Blue
                }
            }

            return bits;
        }

        public void UpdateFromIPersistable(IPersistable data)
        {
            throw new NotImplementedException();
        }

        public void loadImage(IImageParser parser)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
