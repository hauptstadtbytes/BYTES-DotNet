//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.XPath;

//import instanal namespace(s) required
using BYTES.NET.Persistance.API;
using BYTES.NET.Primitives;

namespace BYTES.NET.Persistance
{
    /// <summary>
    /// extension method(s) for types implementing 'ICsvPersistable'
    /// </summary>
    public static class ICsvPersistableExtensions
    {
        /// <summary>
        /// writes the persistable to CSV disk file
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="path"></param>
        /// <param name="hasHeader"></param>
        /// <param name="delimiter"></param>
        public static void WriteToCSV(this ICsvPersistable instance, string path, bool hasHeader = true, char delimiter = ';')
        {
            instance.ToDataTable().ToCSVFile(path, hasHeader, delimiter);
        }

        /// <summary>
        /// reads the persistable from CSV disk file
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="path"></param>
        /// <param name="hasHeader"></param>
        /// <param name="delimiter"></param>
        public static void ReadFromCSV(this ICsvPersistable instance, string path, bool hasHeader = true, char delimiter = ';')
        {
            DataTable tmpData = new DataTable();
            tmpData.FromCSV(path, hasHeader, delimiter);

            instance.UpdateFromDataTable(tmpData);
        }
    }
}
