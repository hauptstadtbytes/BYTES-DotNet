//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;

//import internal namespace(s) required
using BYTES.NET.Primitives;
using BYTES.NET.Persistance.API;

namespace BYTES.NET.Collections
{
    /// <summary>
    /// a XML-serializable dictionary
    /// </summary>
    [Serializable]
    public class ExtendedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable, IXmlPersistable
    {
        #region private variable(s)

        private string _itemName = "Item";
        private string _keyName = "Key";
        private string _valueName = "Value";

        private bool _serialize = true;

        private bool _embedStruc = true;

        #endregion

        #region public properties

        protected virtual string XmlItemName
        {
            get => _itemName;
            set => _itemName = value;
        }

        protected virtual string XmlKeyName
        {
            get => _keyName;
            set => _keyName = value;
        }

        protected virtual string XmlValueName
        {
            get => _valueName;
            set => _valueName = value;
        }

        protected virtual bool Serialize
        {
            get => _serialize;
            set => _serialize = value;
        }

        protected virtual bool EmbedStructure
        {
            get => _embedStruc;
            set => _embedStruc = value;
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
        /// overloaded new instance method, supporting comparer definition
        /// </summary>
        /// <param name="comparer"></param>
        public ExtendedDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        #endregion

        #region public method(s), support 'IXmlSerializable'

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

            //read the (embedded) structure definition(s)
            if (this.EmbedStructure)
            {
                ReadStruc(reader);
            }


            //read the XML data
            if (this.Serialize)
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

                    reader.ReadStartElement(this.XmlItemName);

                    reader.ReadStartElement(this.XmlKeyName);
                    TKey key = (TKey)keySerializer.Deserialize(reader);
                    reader.ReadEndElement();

                    reader.ReadStartElement(this.XmlValueName);
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
            if (this.Serialize)
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

            //write the structure
            if (this.EmbedStructure)
            {
                WriteStruc(writer);
            }


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

        #endregion

        #region public method(s), supporting 'IXmlPersistable'

        /// <summary>
        /// updates the data from 'IPersistable' instance
        /// </summary>
        /// <param name="data"></param>
        public void UpdateFromIPersistable(IPersistable data)
        {
            Dictionary<TKey, TValue> updated = (Dictionary<TKey, TValue>)data;

            this.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in updated)
            {
                this.Add(pair.Key, pair.Value);
            }
        }

        #endregion

        #region protected method(s)

        /// <summary>
        /// writes the structure definition to XML data
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void WriteStruc(XmlWriter writer)
        {
            writer.WriteAttributeString("Meta", "Serialize:" + this.Serialize.ToString() + ",Item:" + XmlItemName + ",Key:" + XmlKeyName + ",Value:" + XmlValueName);
        }

        /// <summary>
        /// reads the structure definition from XML data
        /// </summary>
        /// <param name="reader"></param>
        protected virtual void ReadStruc(XmlReader reader)
        {
            reader.MoveToFirstAttribute();

            Dictionary<string, string> struc = new Dictionary<string, string>();

            foreach (string pairString in reader.GetAttribute("Meta").Split(','))
            {
                KeyValuePair<string, string> pair = pairString.ParseKeyValue();

                if (pair.Key.ToLower() == "serialize")
                {
                    this.Serialize = bool.Parse(pair.Value);
                }
                else if (pair.Key.ToLower() == "item")
                {
                    this.XmlItemName = pair.Value;
                }
                else if (pair.Key.ToLower() == "key")
                {
                    this.XmlKeyName = pair.Value;
                }
                else if (pair.Key.ToLower() == "value")
                {
                    this.XmlValueName = pair.Value;
                }
            }
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// indexes all items by it's roperty value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <remarks>based on <seealso href="https://stackoverflow.com/questions/15341028/check-if-a-property-exists-in-a-classv"/></remarks>
        public Dictionary<T, List<TValue>> IndexByItemProperty<T>(string propertyName)
        {
            Dictionary<T, List<TValue>> output = new Dictionary<T, List<TValue>>();

            System.Type type = typeof(TValue);

            if (type.PropertyExists(propertyName)) //check if the property exists
            {
                foreach (TValue item in this.Values)
                {
                    PropertyInfo prop = type.GetProperty(propertyName);
                    T key = default(T);

                    try
                    {
                        key = (T)Convert.ChangeType(prop.GetValue(item), typeof(T));
                    }
                    catch (Exception ex)
                    {
                        if (typeof(T).Equals(typeof(string)))
                        {
                            key = (T)Convert.ChangeType(prop.GetValue(item).ToString(), typeof(T));
                        }
                    }

                    if (output.ContainsKey(key))
                    {
                        output[key].Add(item);
                    }
                    else
                    {
                        output.Add(key, new List<TValue>() { item });
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// returns the items matching a property value given
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public List<TValue> FilterByItemProperty<T>(string propertyName, T propertyValue)
        {
            Dictionary<T, List<TValue>> indexed = IndexByItemProperty<T>(propertyName);

            if (indexed.ContainsKey(propertyValue))
            {
                return indexed[propertyValue];
            } else
            {
                return new List<TValue>();
            }
        }

        #endregion
    }
}