//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Persistance.API;
using BYTES.NET.IO.Scripting;

namespace BYTES.NET.Test.IO.Scripting
{
    [Serializable]
    public class TestScript : ScriptingToken,IFilePersistable
    {

        private string _instance = Guid.NewGuid().ToString();

        [XmlAttribute(AttributeName = "ID")]
        public override string ID
        {
            get => _instance;
            set => _instance = value;
        }

        public Sequence Sequence { get; set; }

        public TestScript()
        {
            this.Sequence = new Sequence();
        }

        public void Load(IFilePersistable data)
        {
            TestScript tmpScript = (TestScript)data;
            this.Sequence = tmpScript.Sequence;
        }

    }
}
