using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageMagick;

namespace PackageTest_Magick
{
    internal static class Program
    {
        private const int MaxPrint = 8; // only display a few lines to not clutter the screen

        private static readonly string[] Extensions = { ".jpg",".tif", ".tiff" };

        private static void Main()
        {
            // get the base app dir
            IEnumerable<string> folders = new[] { AppContext.BaseDirectory, Directory.GetCurrentDirectory() }
                .Select(Path.GetFullPath)
                .Distinct(StringComparer.OrdinalIgnoreCase);

            // get the pictures
            List<string> files = folders
                .Where(Directory.Exists)
                .SelectMany(f => Directory.GetFiles(f))
                .Where(f => Extensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))
                .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (files.Count == 0)
            {
                Console.WriteLine("No pictures found in: " + AppContext.BaseDirectory);
                return;
            }

            // process the images
            foreach (string file in files)
            {
                try
                {
                    Process(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error at {Path.GetFileName(file)}: {ex.Message}");
                }
            }
        }

        private static void Process(string path)
        {
            using MagickImage image = new MagickImage(path);

            // get the image infos
            int w = (int)image.Width;
            int h = (int)image.Height;
            bool isGray = IsGray(image);

            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"{Path.GetFileName(path)}  |  {w} x {h}  |  " +
                              $"{image.Format}, {image.ColorSpace}  |  {(isGray ? "Grayscale" : "RGB")}");
            Console.WriteLine(new string('=', 60));

            if (isGray)
            {
                PrintMatrix("Gray", ToGrayMatrix(image, w, h));
            }
            else
            {
                byte[,,] rgb = ToRgbMatrix(image, w, h);
                PrintMatrix("R", Channel(rgb, 0));
                PrintMatrix("G", Channel(rgb, 1));
                PrintMatrix("B", Channel(rgb, 2));
            }
            Console.WriteLine();
        }

        private static bool IsGray(MagickImage image) =>
            image.ColorSpace == ColorSpace.Gray ||
            image.ColorType == ColorType.Grayscale ||
            image.ColorType == ColorType.GrayscaleAlpha ||
            image.ColorType == ColorType.Bilevel;

        // convert grayscale image to matrix
        private static byte[,] ToGrayMatrix(MagickImage image, int w, int h)
        {
            // "I" = Intensität, ein Byte pro Pixel, zeilenweise (kein Stride/Padding)
            byte[]? data = image.GetPixels().ToByteArray("I");
            if (data == null) throw new InvalidOperationException("Keine Pixeldaten.");

            byte[,] m = new byte[h, w];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    m[y, x] = data[y * w + x];
            return m;
        }

        // convert RGB image to matrix
        private static byte[,,] ToRgbMatrix(MagickImage image, int w, int h)
        {
            // R,G,B,R,G,B,...
            byte[]? data = image.GetPixels().ToByteArray("RGB");
            if (data == null) throw new InvalidOperationException("No pixeldata.");

            byte[,,] m = new byte[h, w, 3];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int i = (y * w + x) * 3;
                    m[y, x, 0] = data[i];
                    m[y, x, 1] = data[i + 1];
                    m[y, x, 2] = data[i + 2];
                }
            }
            return m;
        }

        private static byte[,] Channel(byte[,,] rgb, int c)
        {
            int h = rgb.GetLength(0), w = rgb.GetLength(1);
            byte[,] m = new byte[h, w];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    m[y, x] = rgb[y, x, c];
            return m;
        }

        private static void PrintMatrix(string name, byte[,] m)
        {
            int h = m.GetLength(0), w = m.GetLength(1);
            int rows = Math.Min(h, MaxPrint), cols = Math.Min(w, MaxPrint);

            Console.WriteLine($"{name} ({rows} x {cols} of {h} x {w}):");
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                    Console.Write(m[y, x].ToString().PadLeft(4));
                Console.WriteLine(cols < w ? "  ..." : "");
            }
            if (rows < h) Console.WriteLine("   ...");
        }
    }
}