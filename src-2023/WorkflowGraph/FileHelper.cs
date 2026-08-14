using System;
using System.Collections.Generic;
using System.Text;

namespace Graph
{
    /// <summary>
    /// Methods that can be used by workflow
    /// </summary>
    public class FileHelper
    {
        public bool fileExists(string filepath) => File.Exists(filepath);
        public void createFile(string filepath)
        {
            string? directory = Path.GetDirectoryName(filepath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory); // legt an, falls nicht vorhanden; wirft nicht, falls schon da
            }
            File.Create(filepath).Dispose();
        }
        public void modifyFile(string filepath, string input) => File.WriteAllText(filepath, input);
        public string readFile(string filepath) => File.ReadAllText(filepath);
    }
}
