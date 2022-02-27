//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.Primitives.Extensions;

namespace BYTES.NET.IO
{
    /// <summary>
    /// IO helper method(s)
    /// </summary>
    public class Helper
    {

        /// <summary>
        /// returns the file system path for the 'BYTES.NET' library assembly
        /// </summary>
        /// <returns></returns>
        public static string GetLibraryAssemblyPath()
        {
            Uri assembly = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase);
            return assembly.LocalPath;
        }

        /// <summary>
        /// returns the BYTES.NET assembly directory's file system path
        /// </summary>
        /// <returns></returns>
        public static string GetLibraryDirPath()
        {
            return Path.GetDirectoryName(GetLibraryAssemblyPath());
        }

        /// <summary>
        /// expands the variables in the input (file system) path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string ExpandPath(string path = null, Dictionary<string, string>? variables = null, bool ignoreCase = true)
        {
            //return the default output
            if (path == null)
            {
                return System.String.Empty;
            }

            //handle (valid) URLs
            if (path.IsWebURL())
            {
                return path;
            }

            //parse the argument(s)
            if (variables == null)
            {
                variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                variables = new Dictionary<string, string>(variables, StringComparer.OrdinalIgnoreCase);
            }

            //add the default variable(s)
            if (!variables.ContainsKey("%BYTES.NET%"))
            {
                variables.Add("%BYTES.NET%", GetLibraryAssemblyPath());
            }

            if (!variables.ContainsKey("%BYTES.NET.DIR%"))
            {
                variables.Add("%BYTES.NET.DIR%", GetLibraryDirPath());
            }

            //expand the variables and return the output value
            //return System.Environment.ExpandEnvironmentVariables(path.ExpandVariables(variables, ignoreCase));
            path = System.Environment.ExpandEnvironmentVariables(path.Expand(variables, ignoreCase));

            Uri tmpPath;

            if (Uri.TryCreate(path, UriKind.Absolute, out tmpPath))
            {
                path = tmpPath.LocalPath;
            }

            return path;

        }

        /// <summary>
        /// expands (file system) paths that (might) contain wildcards
        /// </summary>
        /// <param name="path"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string[] ExpandWildcardPath(string path = null, Dictionary<string, string>? variables = null, bool ignoreCase = true)
        {

            List<string> output = new List<string>();

            if (path != null) //check for non-empty input
            {

                //return the (expanded) input as output, if no wildcard contained
                if (!path.Contains('*'))
                {

                    output.Add(ExpandPath(path, variables, ignoreCase));
                    return output.ToArray();
                }

                //get all instances of '*' and '(\\|\/)'
                int[] asterixIndexes = path.AllIndexesOf('*');
                int[] delimiterIndexes = path.AllIndexesOf(new Regex(@"(\\|\/)"));

                //first asterix is behind last delimiter (e.g. 'D:\Test\myFolder\*.dll' or 'D:\Test\my*\*.dll')
                if ((asterixIndexes[0] > delimiterIndexes[delimiterIndexes.Length - 1]) & path.Substring(delimiterIndexes[delimiterIndexes.Length - 1]).Contains('.'))
                {
                    string searchPattern = path.Substring(delimiterIndexes[delimiterIndexes.Length - 1] + 1);

                    foreach (string dirPath in ExpandWildcardPath(path.Substring(0, delimiterIndexes[delimiterIndexes.Length - 1]), variables, ignoreCase))
                    {

                        DirectoryInfo dirInfo = new DirectoryInfo(dirPath);

                        if (dirInfo.Exists)
                        {
                            foreach (FileInfo file in dirInfo.GetFiles(searchPattern))
                            {

                                foreach (string outputPath in ExpandWildcardPath(file.FullName, variables, ignoreCase))
                                {
                                    output.Add(outputPath);
                                }

                            }
                        }
                        
                    }

                    return output.ToArray();

                }

                //first asterix is a folder search pattern (e.g. 'D:\Test\my*\' or 'D:\Test\my*Folder')
                if (delimiterIndexes[0] < asterixIndexes[0])
                {

                    //get the (parent) folder path and (directory) search pattern
                    int patternStart = 0;
                    int patternEnd = path.Length - 1;

                    foreach (int delimiterIndex in delimiterIndexes)
                    {

                        if (delimiterIndex < asterixIndexes[0])
                        {
                            patternStart = delimiterIndex;
                        }

                        if (delimiterIndex > asterixIndexes[0] & delimiterIndex <= path.Length - 1)
                        {
                            patternEnd = delimiterIndex;
                        }

                    }

                    string dirPath = ExpandPath(path.Substring(0, patternStart), variables, ignoreCase);

                    string searchPattern = path.Substring(patternStart + 1, patternEnd - patternStart);
   
                    if (searchPattern.EndsWith("\\") ^ searchPattern.EndsWith("/"))
                    {
                        searchPattern = searchPattern.Substring(0, searchPattern.Length - 1);
                    }

                    string pathExtension = System.String.Empty;
                    if (patternEnd != path.Length - 1)
                    {
                        pathExtension = path.Substring(patternEnd);
                    }

                    if (searchPattern.Contains("\\"))
                    {
                        string[] split = searchPattern.Split('\\');
                        searchPattern = split[0];

                        if (split.Length > 1)
                        {
                            for (int i = 1; i <= split.Length - 1; i++)
                            {
                                pathExtension = "\\" + split[i] + pathExtension;
                            }

                        }

                    }

                    //add path(s) to all files matching the search pattern
                    DirectoryInfo dirInfo = new DirectoryInfo(dirPath);

                    foreach (DirectoryInfo dir in dirInfo.GetDirectories(searchPattern))
                    {

                        foreach (string outputPath in ExpandWildcardPath((dir.FullName + pathExtension), variables, ignoreCase))
                        {
                            output.Add(outputPath);
                        }

                    }

                }

            }

            //return the default output
            return output.ToArray();

        }

        /// <summary>
        /// overloaded method, expanding wildcard paths from string array input
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string[] ExpandWildcardPath(string[]? paths = null, Dictionary<string, string>? variables = null, bool ignoreCase = true)
        {
            List<string> output = new List<string>();

            if (paths != null)
            {

                foreach (string path in paths)
                {

                    foreach (string expandedPath in ExpandWildcardPath(path, variables, ignoreCase))
                    {

                        if (!output.Contains(expandedPath))
                        {
                            output.Add(expandedPath);
                        }

                    }

                }

            }

            return output.ToArray();

        }

    }
}
