//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;



//import internal namespace(s) required

namespace BYTES.NET.Automation.Scripting
{
    [Serializable]
    [XmlRoot("Call")]
    public class Call
    {
        #region private variable(s)

        private Guid _id = Guid.NewGuid();
        private ScriptingEntityMetadata _metadata = new ScriptingEntityMetadata();
        private ScriptingEntityArguments _arguments = new ScriptingEntityArguments();

        #endregion

        #region public properties

        [XmlAttribute("ID")]
        public Guid ID { get => _id; set => _id = value; }

        [XmlElement("Metadata")]
        public ScriptingEntityMetadata Metadata { get => _metadata; set => _metadata = value; }

        [XmlElement("Arguments")]
        public ScriptingEntityArguments Arguments { get => _arguments; set => _arguments = value; }

        #endregion
    }
}
