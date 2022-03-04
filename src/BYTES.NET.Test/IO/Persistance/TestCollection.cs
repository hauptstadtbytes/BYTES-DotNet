using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BYTES.NET.IO.Persistance.API;

namespace BYTES.NET.Test.IO.Persistance
{
    [XmlRoot("TheCollection")]
    [Serializable]
    public class TestCollection : IFilePersistable
    {
        private string _text = String.Empty;
        private int _number = 0;

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        public int Number
        {
            get => _number;
            set => _number = value;
        }

        public TestCollection()
        {
        }

        public TestCollection(string text)
        {
            this.Text = text;
        }

        public void FromIPersistable(IFilePersistable data)
        {
            TestCollection tmp = (TestCollection)data;

            this.Text = tmp.Text;
            this.Number = tmp.Number;
        }

        public DataTable ToTable()
        {
            throw new NotImplementedException();
        }

        public void FromTable(DataTable data)
        {
            throw new NotImplementedException();
        }
    }
}
