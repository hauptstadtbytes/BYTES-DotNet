//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

//import instanal namespace(s) required
using BYTES.NET.IO;
using BYTES.NET.Persistance.API;

namespace BYTES.NET.Persistance
{
    /// <summary>
    /// extension method(s) for types implementing 'IXmlSerializable'
    /// </summary>
    public static class IXmlPersistableExtensions
    {
        #region public method(s)

        /// <summary>
        /// XML serializes the data to 'IO.Stream'
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="stream"></param>
        public static void WriteToXML(this IXmlPersistable instance, ref Stream stream)
        {
            if (instance.GetType().IsSerializable)
            {
                XmlSerializer serializer = new XmlSerializer(instance.GetType());
                StreamWriter writer = new StreamWriter(stream);

                serializer.Serialize(writer, instance);

                writer.Close();
            }
            else
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
        public static void WriteToXML(this IXmlPersistable instance, string path, Dictionary<string, string>? variables = null, bool ignoreCase = true)
        {
            //parse the argument(s)
            path = PrepareForWriting(instance.GetType(), path, variables, ignoreCase, (path, type) =>
            {
                return type.IsSerializable;
            });

            //write the output file
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                Stream outStream = stream;
                instance.WriteToXML(ref outStream);

                stream.Close();
            }
        }

        /// <summary>
        /// reads data from 'IO.Stream' using XML de-serialization
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="stream"></param>
        public static void ReadFromXML(this IXmlPersistable instance, ref Stream stream)
        {
            if (instance.GetType().IsSerializable)
            {
                XmlSerializer serializer = new XmlSerializer(instance.GetType());
                StreamReader reader = new StreamReader(stream);

                IXmlPersistable tmp = (IXmlPersistable)serializer.Deserialize(reader);
                instance.UpdateFromIPersistable(tmp);

                reader.Close();
            }
            else
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
        public static void ReadFromXML(this IXmlPersistable instance, string path, Dictionary<string, string>? variables = null, bool ignoreCase = true)
        {
            //parse the argument(s)
            path = path.ExpandPath(variables, ignoreCase);

            if (!File.Exists(path))
            {
                throw new ArgumentException("Unable to find file '" + path + "'");
            }

            //read the data
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                Stream instream = stream;
                instance.ReadFromXML(ref instream);

                stream.Close();
            }
        }

        #endregion

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
        private static string PrepareForWriting(Type dataType, string path, Dictionary<string, string>? variables, bool ignoreCase, Func<string, Type, bool>? validationCallback = null)
        {
            //parse the argument(s)
            path = path.ExpandPath(variables, ignoreCase);

            bool createDirectory = true;

            if (validationCallback != null)
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
