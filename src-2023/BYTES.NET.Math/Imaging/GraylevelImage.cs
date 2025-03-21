using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using BYTES.NET.Math.Imaging.API;

using BYTES.NET.Persistance.API;

namespace BYTES.NET.Math.Imaging
{
    /// <summary>
    /// Represents a grayscale image.
    /// </summary>
    public class GraylevelImage : IImage
    {
        #region Private Fields

        private string _name = Guid.NewGuid().ToString();
        private DataMatrix<int> _pxValues;

        #endregion

        #region Public Properties Implementing IImage

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        #endregion

        #region Public Properties

        public DataMatrix<int> Values => _pxValues;
        public int Width => _pxValues.XLength;
        public int Height => _pxValues.YLength;

        #endregion

        #region Constructors

        public GraylevelImage(int[,] px)
        {
            _pxValues = new DataMatrix<int>(px);
        }

        public GraylevelImage(int[] values, int width, int height)
        {
            var pxValues = new int[width, height];

            for (int col = 0; col < width; col++)
            {
                for (int row = 0; row < height; row++)
                {
                    pxValues[col, row] = values[col * height + row];
                }
            }

            _pxValues = new DataMatrix<int>(pxValues);
        }

        #endregion

        #region Public Methods Implementing IImage

        /// <summary>
        /// Returns raw grayscale image data.
        /// </summary>
        public byte[] GetRawImageData(out int width, out int height, out int stride)
        {
            width = Width;
            height = Height;

            // Ensure 4-byte alignment
            stride = width;
            int numPaddingBytes = 0;
            while (stride % 4 != 0)
            {
                stride++;
                numPaddingBytes++;
            }

            byte[] bits = new byte[height * stride];

            int? min = _pxValues.Minimum();
            int? max = _pxValues.Maximum();

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int pos = row * stride + col;
                    if (max <= 1) // Binary image
                    {
                        bits[pos] = (byte)(_pxValues[col +1, row +1] * 255);
                    }
                    else if (max <= 255) // 8-bit grayscale
                    {
                        bits[pos] = (byte)_pxValues[col + 1, row + 1];
                    }
                    else // 12- to 16-bit grayscale
                    {
                        bits[pos] = (byte)((_pxValues[col, row] - min) / (float)(max - min) * 255);
                    }
                }
            }

            return bits;
        }

        /// <summary>
        /// Generates PNG byte array.
        /// </summary>
        public byte[] GetPngBytes()
        {
            int width = Width;
            int height = Height;

            using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed))
            {
                // Set grayscale palette
                ColorPalette palette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    palette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = palette;

                BitmapData data = bitmap.LockBits(
                    new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format8bppIndexed);

                byte[] rawBytes = GetRawImageData(out _, out _, out int stride);
                IntPtr ptr = data.Scan0;
                System.Runtime.InteropServices.Marshal.Copy(rawBytes, 0, ptr, rawBytes.Length);
                bitmap.UnlockBits(data);

                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        public void UpdateFromIPersistable(IPersistable data)
        {
            throw new NotImplementedException();
        }

        public void loadImage(IImageParser parser)
        {
            IImage[] images = parser.Load(Name);

            if (images.Length > 0 && images[0] is GraylevelImage grayImage)
            {
                int width = grayImage.Width;
                int height = grayImage.Height;
                int[,] extractedValues = new int[width, height];

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        extractedValues[x, y] = grayImage.Values[x, y]; // Extract values manually
                    }
                }

                _pxValues = new DataMatrix<int>(extractedValues);
            }
        }

        #endregion
    }
}
