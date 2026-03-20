//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BYTES.NET.IO
{
    /// <summary>
    /// 'FileInfo' type extension method(s)
    /// </summary>
    public static class FileInfoExtensions
    {
        #region public method(s)

        /// <summary>
        /// reads all lines of a text file to string array, opening the file in read-only mode only
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <remarks>file should be readable, although being used by another application; see also 'https://stackoverflow.com/questions/12744725/how-do-i-perform-file-readalllines-on-a-file-that-is-also-open-in-excel'</remarks>
        public static string[] ReadAllAllLines(this FileInfo file)
        {
            using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    List<string> lines = new List<string>();

                    while (!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine());
                    }

                    return lines.ToArray();
                }
            }
        }

        #endregion
    }
}
