//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Persistance.API;

namespace BYTES.NET.Tests.Persistable
{
     
    [Serializable] //required for XML serialization
    [XmlRoot("SampleObject")] //required for XML serialization
    public class SamplePersistableobject : MarshalByRefObject, IXmlPersistable, ICsvPersistable //'MarshalByRefObject' required for parent reference object
    {
        #region private variable(s)

        private string _name = string.Empty;
        private int _id;

        private SamplePersistableobject? _parent = null;

        #endregion

        #region public properties

        public string Group { get; set; }

        public string Name { get => _name; set => _name = value; }

        public int ID { get => _id; set => _id = value; }

        public SamplePersistableobject Parent => _parent;

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <remarks>required for XML serialization</remarks>
        public SamplePersistableobject()
        {
        }

        /// <summary>
        /// overloaded new instance method, accepting a parent object
        /// </summary>
        /// <param name="parent"></param>
        public SamplePersistableobject(SamplePersistableobject parent)
        {
            _parent = parent;
        }

        #endregion

        #region public method(s) supporting 'IPersistable' (inherited by 'IXmlpersistable' or 'ICsvPersistable')

        public void UpdateFromIPersistable(IPersistable data)
        {
            SamplePersistableobject tmp = (SamplePersistableobject)data;

            _name = tmp.Name;
            _id = tmp.ID;
            this.Group = tmp.Group;
            _parent = tmp.Parent;
        }

        #endregion

        #region public method(s) supporting 'IXmlpersistable'

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Data");

            writer.WriteAttributeString("ID", ID.ToString());
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("Group", Group);

            writer.WriteEndElement();
        }

        #endregion

        #region public method(s) supporting 'ICsvPersistable'

        public DataTable ToDataTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Group", typeof(string));
            table.Columns.Add("Parent", typeof(int));

            DataRow row = table.NewRow();

            row["Name"] = _name;
            row["ID"] = _id;
            row["Group"] = this.Group;

            if (_parent != null)
            {
                row["Parent"] = _parent.ID;
            }

            table.Rows.Add(row);

            return table;
        }

        public void UpdateFromDataTable(DataTable data)
        {
            foreach (DataRow row in data.Rows)
            {
                _name = row["Name"].ToString();
                _id = int.Parse(row["ID"].ToString());
                this.Group = row["Group"].ToString();

                if (row["Parent"] != null)
                {
                    _parent = new SamplePersistableobject() { ID = int.Parse(row["Parent"].ToString()) };
                }
            }
        }

        #endregion

        #region public method(s)

        public bool IsMatch(string name)
        {
            if (Name.Length > 3)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
