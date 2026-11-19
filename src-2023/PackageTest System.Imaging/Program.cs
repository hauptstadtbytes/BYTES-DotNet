using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PackageTest_System.Imaging
{
    internal static class Program
    {
        private const int MaxPrint = 8; // print a part of the matrix to not clutter screen

        private static readonly string[] Extensions = { ".jpg", ".tif", ".tiff" };

        private static void Main()
        {
            // get the base app folder
            IEnumerable<string> folders = new[] { AppContext.BaseDirectory, Directory.GetCurrentDirectory() }
                .Select(Path.GetFullPath)
                .Distinct(StringComparer.OrdinalIgnoreCase);

            // get files
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

        // process the image
        private static void Process(string path)
        {
            BitmapSource src = LoadFrame(path);
            bool isGray = IsGray(src.Format);

            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"{Path.GetFileName(path)}  |  {src.PixelWidth} x {src.PixelHeight}  |  " +
                              $"Format: {src.Format}  |  {(isGray ? "Grayscale" : "RGB")}");
            Console.WriteLine(new string('=', 60));

            if (isGray)
            {
                PrintMatrix("Gray", ToGrayMatrix(src));
            }
            else
            {
                byte[,,] rgb = ToRgbMatrix(src);
                PrintMatrix("R", Channel(rgb, 0));
                PrintMatrix("G", Channel(rgb, 1));
                PrintMatrix("B", Channel(rgb, 2));
            }
            Console.WriteLine();
        }

        // Load the image
        // automatically recognizes the format
        private static BitmapSource LoadFrame(string path)
        {
            using FileStream stream = File.OpenRead(path);

            // create a decoder for the image
            BitmapDecoder decoder = BitmapDecoder.Create(
                stream,
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.OnLoad);

            // return the instance of the decoder
            return decoder.Frames[0];
        }

        // set picture to grayscale if we want it in grayscale
        private static bool IsGray(PixelFormat f) =>
            f == PixelFormats.Gray8 || f == PixelFormats.Gray16 ||
            f == PixelFormats.Gray4 || f == PixelFormats.Gray2 ||
            f == PixelFormats.BlackWhite || f == PixelFormats.Gray32Float;

        // convert grayscale to matrix
        private static byte[,] ToGrayMatrix(BitmapSource src)
        {
            if (src.Format != PixelFormats.Gray8)
                src = new FormatConvertedBitmap(src, PixelFormats.Gray8, null, 0);

            int w = src.PixelWidth, h = src.PixelHeight;
            int stride = w;
            byte[] buffer = new byte[h * stride];
            src.CopyPixels(buffer, stride, 0);

            byte[,] m = new byte[h, w];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    m[y, x] = buffer[y * stride + x];
            return m;
        }

        // convert RGB to matrix
        private static byte[,,] ToRgbMatrix(BitmapSource src)
        {
            if (src.Format != PixelFormats.Bgr24)
                src = new FormatConvertedBitmap(src, PixelFormats.Bgr24, null, 0);

            int w = src.PixelWidth, h = src.PixelHeight;
            int stride = (w * 24 + 7) / 8;
            var buffer = new byte[h * stride];
            src.CopyPixels(buffer, stride, 0);

            byte[,,] m = new byte[h, w, 3];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int i = y * stride + x * 3;
                    m[y, x, 0] = buffer[i + 2]; // R
                    m[y, x, 1] = buffer[i + 1]; // G
                    m[y, x, 2] = buffer[i];     // B
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
                Console.WriteLine(rows < h || cols < w ? (cols < w ? "  ..." : "") : "");
            }
            if (rows < h) Console.WriteLine("   ...");
        }
    }
}