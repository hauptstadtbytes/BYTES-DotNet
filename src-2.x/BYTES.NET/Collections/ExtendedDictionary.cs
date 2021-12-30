//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

//import internal namespace(s) required
using BYTES.NET.Primitives.Extensions;
using BYTES.NET.IO.Persistance.API;

namespace BYTES.NET.Collections
{
    /// <summary>
    /// a XML-serializable dictionary implementation
    /// </summary>
    [Serializable]
    public class ExtendedDictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>, IXmlSerializable, IFilePersistable
    {
        #region private variable(s)

        private char _strcDelimiter = ',';

        #endregion

        #region protected properties

        protected virtual string XmlItemName
        {
            get => "Item";
        }

        protected virtual string XmlKeyName
        {
            get => "Key";
        }

        protected virtual string XmlValueName
        {
            get => "Value";
        }

        protected virtual bool Serialize
        {
            get => true;
        }

        #endregion

        #region public properties

        [XmlIgnore]
        public char StructureDelimiter
        {
            get => _strcDelimiter;
            set
            {
                _strcDelimiter = value;
            }
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <remarks>required for XML serialization</remarks>
        public ExtendedDictionary() : base()
        {
        }

        /// <summary>
        /// overloaded new instance method, supporting to define a specific comparer
        /// </summary>
        /// <param name="comparer"></param>
        public ExtendedDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        #endregion

        #region public method(s) implementing 'IXmlSerializable'

        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// reads the data from XML (using XML reader)
        /// </summary>
        /// <param name="reader"></param>
        /// <exception cref="ArgumentException"></exception>
        public void ReadXml(XmlReader reader)
        {
            //get the serilization tag
            reader.MoveToFirstAttribute();
            bool serialize = (bool)Convert.ChangeType(reader.GetAttribute("Serialize"), typeof(bool));

            //get the structure tag
            Dictionary<string, string> struc = new Dictionary<string, string>();

            foreach (string pairString in reader.GetAttribute("Structure").Split(_strcDelimiter))
            {
                KeyValuePair<string, string> pair = pairString.ParseKeyValue();
                struc.Add(pair.Key.ToLower(), pair.Value);
            }

            //read the XML data
            if (serialize)
            {
                XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
                XmlSerializer valSerializer = new XmlSerializer(typeof(TValue));

                bool wasEmpty = reader.IsEmptyElement;
                reader.Read();

                if (wasEmpty)
                {
                    return;
                }

                while (reader.NodeType != XmlNodeType.EndElement)
                {

                    reader.ReadStartElement(struc["item"]);

                    reader.ReadStartElement(struc["key"]);
                    TKey key = (TKey)keySerializer.Deserialize(reader);
                    reader.ReadEndElement();

                    reader.ReadStartElement(struc["value"]);
                    TValue value = (TValue)valSerializer.Deserialize(reader);
                    reader.ReadEndElement();

                    Add(key, value);

                    reader.ReadEndElement();
                    reader.MoveToContent();

                }

                reader.ReadEndElement();
            }
            else
            {
                reader.MoveToContent();
                string fileContent = reader.ReadInnerXml();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Dictionary>" + fileContent + "</Dictionary>");

                foreach (XmlElement node in doc.SelectNodes(".//Dictionary/*"))
                {

                    XmlAttributeCollection attributes = node.Attributes;
                    if (attributes.Count < 1)
                    {
                        throw new ArgumentException("Key attribute missing");
                    }

                    TKey key = (TKey)Convert.ChangeType(attributes[0].Value, typeof(TKey));
                    TValue value = (TValue)Convert.ChangeType(node.InnerText, typeof(TValue));

                    Add(key, value);

                }

            }
        }

        /// <summary>
        /// writes the data to XML (using XML writer)
        /// </summary>
        /// <param name="writer"></param>
        /// <exception cref="Exception"></exception>
        public void WriteXml(XmlWriter writer)
        {
            //validate the data type(s) for serialization
            if (Serialize)
            {

                if (!typeof(TKey).IsSerializable)
                {
                    throw new Exception("Unable to serialize key of type '" + typeof(TKey).ToString() + "': Object is not tagged as serializable");
                }

                if (!typeof(TValue).IsSerializable)
                {
                    throw new Exception("Unable to serialize value of type '" + typeof(TValue).ToString() + "': Object is not tagged as serializable");
                }

            }

            //write the serialization tag
            writer.WriteAttributeString("Serialize", Serialize.ToString());

            //write the structure tag
            writer.WriteAttributeString("Structure", "Item:" + XmlItemName + ",Key:" + XmlKeyName + ",Value:" + XmlValueName);

            //write the XML data
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in Keys)
            {

                writer.WriteStartElement(XmlItemName);

                if (!Serialize)
                {

                    writer.WriteAttributeString(XmlKeyName, key.ToString());
                    writer.WriteString(this[key].ToString());

                }
                else
                {

                    writer.WriteStartElement(XmlKeyName);
                    keySerializer.Serialize(writer, key);
                    writer.WriteEndElement();

                    writer.WriteStartElement(XmlValueName);
                    valSerializer.Serialize(writer, this[key]);
                    writer.WriteEndElement();

                }

                writer.WriteEndElement();

            }
        }

        #region public method(s) implementing 'IFilePersistable'

        /// <summary>
        /// loads the data from a 'IFilePersistable' instance
        /// </summary>
        /// <param name="data"></param>
        /// <remarks>there is no dedicated data type handling implemented</remarks>
        public void Load(IFilePersistable data)
        {
            Dictionary<TKey, TValue> updated = (Dictionary<TKey, TValue>)data;

            this.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in updated)
            {
                this.Add(pair.Key, pair.Value);
            }
        }

        #endregion
    }

    #endregion
}
