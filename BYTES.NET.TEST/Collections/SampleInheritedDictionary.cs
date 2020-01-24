//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

using System.IO;

//import namespace(s) required from 'BYTES.NET' library
using BYTES.NET.Collections;

namespace BYTES.NET.TEST.Collections
{

    [Serializable]
    [XmlType("InheritedDictionary")]
    public class SampleInheritedDictionary : XMLSerializableDictionary<int,SampleDataType>
    {

        #region public new instance method(s)

        public SampleInheritedDictionary()
        {
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// method reading the data from disk
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public void Read(string path)
        {

            FileStream inFile = new FileStream(path, FileMode.Open);

            XMLSerializableDictionary<int, SampleDataType> readDic = new XMLSerializableDictionary<int, SampleDataType>();
            readDic = readDic.Read(ref inFile);

            this.Clear();

            foreach(int index in readDic.Keys)
            {

                this.Add(index, readDic[index]);

            }

        }

        #endregion

    }

}
