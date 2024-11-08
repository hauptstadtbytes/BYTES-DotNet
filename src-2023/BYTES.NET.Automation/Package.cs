//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

//import .net namespace(s) required
using BYTES.NET.Persistance;

//import internal namespace(s) required
using BYTES.NET.Automation.Scripting;

namespace BYTES.NET.Automation
{
    public class Package
    {
        #region private variable(s)

        List<Script> _scripts = new List<Script>();

        #endregion

        #region public properties

        public List<Script> Scripts { get => _scripts; set => _scripts = value; }

        #endregion

        #region public method(s)

        /// <summary>
        /// writes the script package to disk file
        /// </summary>
        /// <param name="filePath"></param>
        /// <remarks>based on the article found at 'https://code-maze.com/csharp-zip-files/'</remarks>
        public void Write(string filePath)
        {
            //delete an older veraion (if existing) of the file
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (ZipArchive archive = ZipFile.Open(@filePath, ZipArchiveMode.Create)){ //create a new archive (file)

                //write the script file(s)
                foreach (Script script in _scripts)
                {
                    //create a new entry
                    ZipArchiveEntry entry = archive.CreateEntry("scripts/" + script.ID + ".scrptx");

                    //write the data to entry item (using stream)
                    Stream stream = entry.Open();

                    script.WriteToXML(ref stream); //use the extension method for 'IXmlPersistables'

                    stream.Close();
                }

            }

        }

        #endregion

    }
}
