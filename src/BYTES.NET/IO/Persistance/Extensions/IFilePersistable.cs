//using .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Data;

//import internal namespace(s) required
using BYTES.NET.Primitives.Extensions;

namespace BYTES.NET.IO.Persistance.Extensions
{
    public static class IFilePersistable
    {
        /// <summary>
        /// XML serializes the data to 'IO.Stream'
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="stream"></param>
        public static void WriteToXML(this API.IFilePersistable instance, ref Stream stream)
        {
            if (instance.GetType().IsSerializable)
            {
                XmlSerializer serializer = new XmlSerializer(instance.GetType());
                StreamWriter writer = new StreamWriter(stream);

                serializer.Serialize(writer, instance);

                writer.Close();
            } else
            {
                throw new ArgumentException("Data type '" + instance.GetType().ToString() + "' is not marked as serializable");
            }
            
        }

        /// <summary>
        /// writes the data to XML disk file
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="path"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        public static void WriteToXML(this API.IFilePersistable instance, string path, Dictionary<string, string>? variables = null, bool ignoreCase = true)
        {
            //parse the argument(s)
            path = PrepareForWriting(instance.GetType(), path, variables, ignoreCase, (path, type) => {
                return type.IsSerializable;
            });

            //write the output file
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                Stream outStream = (Stream)stream;
                instance.WriteToXML(ref outStream);

                stream.Close();
            }
        }

        /// <summary>
        /// reads the data from 'IO.Stream' using XML de-serialization
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="stream"></param>
        public static void ReadFromXML(this API.IFilePersistable instance, ref Stream stream)
        {
            if (instance.GetType().IsSerializable)
            {
                XmlSerializer serializer = new XmlSerializer(instance.GetType());
                StreamReader reader = new StreamReader(stream);

                API.IFilePersistable tmp = (API.IFilePersistable)serializer.Deserialize(reader);
                instance.FromIPersistable(tmp); //update the instance data

                reader.Close();
            } else
            {
                throw new ArgumentException("Data type '" + instance.GetType().ToString() + "' is not marked as serializable");
            }
        }

        /// <summary>
        /// reads the data from XML disk file
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="path"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ReadFromXML(this API.IFilePersistable instance, string path, Dictionary<string, string>? variables = null, bool ignoreCase = true)
        {
            //parse the argument(s)
            path = Helper.ExpandPath(path, variables, ignoreCase);

            if (!File.Exists(path))
            {
                throw new ArgumentException("Unable to find file '" + path + "'");
            }

            //read the data
            using(FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                Stream instream = (Stream)stream;
                instance.ReadFromXML(ref instream);

                stream.Close();
            }
        }

        /// <summary>
        /// writes the data to CSV disk file
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="path"></param>
        /// <param name="hasHeader"></param>
        /// <param name="delimiter"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        public static void WriteToCSV(this API.IFilePersistable instance, string path, bool hasHeader = true, char delimiter = ';', Dictionary<string, string>? variables = null, bool ignoreCase = true )
        {
            //parse the argument(s)
            path = PrepareForWriting(instance.GetType(),path,variables,ignoreCase);

            //get the data table
            DataTable table = instance.ToTable();

            //write to disk file
            table.ToCSVFile(path,hasHeader,delimiter);
        }

        /// <summary>
        /// reads data from CSV disk file
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="path"></param>
        /// <param name="hasHeader"></param>
        /// <param name="delimiter"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        public static void ReadFromCSV(this API.IFilePersistable instance, string path, bool hasHeader = true, char delimiter = ';', Dictionary<string, string>? variables = null, bool ignoreCase = true)
        {
            //parse the argument(s)
            path = Helper.ExpandPath(path, variables, ignoreCase);

            //read the data
            DataTable table = new DataTable();
            table.FromCSV(path);

            //update the 'IFilePersistable' instance
            instance.FromTable(table);
        }

        #region private static method(s)

        /// <summary>
        /// parses the path given and prepares the file system for writing
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="path"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="validationCallback"></param>
        /// <returns></returns>
        private static string PrepareForWriting(Type dataType, string path, Dictionary<string, string>? variables, bool ignoreCase, Func<string,Type,bool>? validationCallback = null)
        {
            //parse the argument(s)
            path = Helper.ExpandPath(path, variables, ignoreCase);

            bool createDirectory = true;

            if(validationCallback != null)
            {
                createDirectory = validationCallback(path, dataType);
            }

            //create the output directory (if required)
            if (createDirectory)
            {
                string dirPath = path.Substring(0, path.LastIndexOf("\\"));

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }

            //return the output value
            return path;
        }
        #endregion
    }
}
