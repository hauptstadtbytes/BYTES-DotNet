using System;
using System.Collections.Generic;
using System.Text;

namespace WorkflowcoreLib
{
    public class FileHelper
    {
        public bool fileExists(string filepath) => File.Exists(filepath);
        public void createFile(string filepath) => File.Create(filepath).Dispose();
        public void modifyFile(string filepath, string input) => File.WriteAllText(filepath, input);
        public string readFile(string filepath) => File.ReadAllText(filepath);
    }
}
