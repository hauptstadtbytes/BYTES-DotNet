//using .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

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
            XmlSerializer serializer = new XmlSerializer(instance.GetType());
            StreamWriter writer = new StreamWriter(stream);

            serializer.Serialize(writer, instance);

            writer.Close();
        }

        /// <summary>
        /// XML serializes the data to disk file
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="path"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        public static void WriteToXML(this API.IFilePersistable instance, string path, Dictionary<string, string> variables = null, bool ignoreCase = true)
        {
            //parse the argument(s)
            path = Helper.ExpandPath(path, variables, ignoreCase);

            //create the output directory (if required)
            string dirPath = path.Substring(0, path.LastIndexOf("\\"));

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            //write the output file
            FileStream outFile = new FileStream(path, FileMode.OpenOrCreate);
            Stream outStream = (Stream)outFile;
            instance.WriteToXML(ref outStream);
            outFile.Close();
        }

        /// <summary>
        /// reads the data from 'IO.Stream' using XML de-serialization
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="stream"></param>
        public static void ReadFromXML(this API.IFilePersistable instance, ref Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(instance.GetType());
            StreamReader reader = new StreamReader(stream);

            API.IFilePersistable tmp = (API.IFilePersistable)serializer.Deserialize(reader);
            instance.Load(tmp); //update the instance data

            reader.Close();
        }

        /// <summary>
        /// reads the data from disk file
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="path"></param>
        /// <param name="variables"></param>
        /// <param name="ignoreCase"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ReadFromXML(this API.IFilePersistable instance, string path, Dictionary<string, string> variables = null, bool ignoreCase = true)
        {
            //parse the argument(s)
            path = Helper.ExpandPath(path, variables, ignoreCase);

            if (!File.Exists(path))
            {
                throw new ArgumentException("Unable to find file '" + path + "'");
            }

            //read the data
            FileStream stream = new FileStream(path, FileMode.Open);
            Stream instream = (Stream)stream;
            instance.ReadFromXML(ref instream);
            stream.Close();
        }
    }
}
