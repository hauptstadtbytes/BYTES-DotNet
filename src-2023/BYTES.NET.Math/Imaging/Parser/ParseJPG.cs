using System;
using System.Collections.Generic;
using System.IO;
using SkiaSharp;
using BYTES.NET.Math.Imaging.API;

namespace BYTES.NET.Math.Imaging.Parser
{
    //[ImageParserMetadata(ID = "BYTES_ImageParser_JPG", Name = "JPG Image File Parser", FileExtensions = new[] { "JPG" })]
    public class ParseJPG : IImageParser
    {
        #region Public Methods Implementing the IImageParser Interface

        /// <summary>
        /// Method loading the image(s) from disk file.
        /// </summary>
        /// <param name="diskPath">The path to the image file on disk.</param>
        /// <returns>An array of IImage instances.</returns>
        public IImage[] Load(string diskPath)
        {
            if (!File.Exists(diskPath))
            {
                throw new FileNotFoundException("The specified file does not exist.", diskPath);
            }

            try
            {
                using (var stream = File.OpenRead(diskPath))
                using (var skBitmap = SKBitmap.Decode(stream))
                {
                    int width = skBitmap.Width;
                    int height = skBitmap.Height;

                    int[,] redValues = new int[width, height];
                    int[,] greenValues = new int[width, height];
                    int[,] blueValues = new int[width, height];

                    // Get the pixel data
                    for (int i = 0; i < width; i++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            var pixel = skBitmap.GetPixel(i, k);
                            redValues[i, k] = pixel.Red;
                            greenValues[i, k] = pixel.Green;
                            blueValues[i, k] = pixel.Blue;
                        }
                    }

                    var red = new GraylevelImage(redValues) { Name = "The red channel" };
                    var green = new GraylevelImage(greenValues) { Name = "The green channel" };
                    var blue = new GraylevelImage(blueValues) { Name = "The blue channel" };

                    var output = new List<IImage>
                    {
                        red,
                        green,
                        blue,
                        new RGBImage(ref red, ref green, ref blue) { Name = "The RGB image" }
                    };

                    return output.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while loading the image.", ex);
            }
        }

        #endregion
    }
}
