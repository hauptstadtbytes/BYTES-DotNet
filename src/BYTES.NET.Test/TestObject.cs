//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

//import namespace(s) required from 'BYTEST.NET' framework
using BYTES.NET.IO.Persistance.API;

namespace BYTES.NET.Test
{
    [XmlRoot("TheObject")]
    [Serializable]
    public class TestObject : MarshalByRefObject, IFilePersistable
    {
        #region private variable(s)

        private string _name = string.Empty;
        private int _id;

        private TestObject? _parent = null;

        #endregion

        #region public properties

        public string Group { get; set; } 

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public int ID {
            get => _id;
            set => _id = value;
        }

        public TestObject Parent => _parent;

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <remarks>required for XML serialization</remarks>
        public TestObject()
        {
        }

        /// <summary>
        /// overloaded new instance method, accepting a parent object
        /// </summary>
        /// <param name="parent"></param>
        public TestObject(TestObject parent)
        {
            _parent = parent;
        }

        #endregion

        #region public method(s), implementing 'IFilePersistable'

        public void FromIPersistable(IFilePersistable data)
        {
            TestObject tmp = (TestObject)data;

            _name = tmp.Name;
            _id = tmp.ID;
            this.Group = tmp.Group;
            _parent = tmp.Parent;
        }

        public DataTable ToTable()
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

            if(_parent != null)
            {
                row["Parent"] = _parent.ID;
            }
            
            table.Rows.Add(row);

            return table;
        }

        public void FromTable(DataTable data)
        {
            foreach(DataRow row in data.Rows)
            {
                _name = row["Name"].ToString();
                _id = int.Parse(row["ID"].ToString());
                this.Group = row["Group"].ToString();

                if(row["Parent"] != null)
                {
                    _parent = new TestObject() { ID = int.Parse(row["Parent"].ToString())};
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
