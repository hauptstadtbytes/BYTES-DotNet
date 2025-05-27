//import (default) .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BYTES.NET.Automation.Scripting
{
    [Serializable]
    [XmlRoot("Call")]
    public class MethodCall
    {
        #region private variable(s)

        private Guid _id = Guid.NewGuid();

        private ScriptingEntityMetadata _metadata = new ScriptingEntityMetadata();
        private ScriptingEntityArguments _arguments = new ScriptingEntityArguments();

        private string _method = string.Empty;

        #endregion

        #region public properties

        [XmlAttribute("ID")]
        public Guid ID { get => _id; set => _id = value; }

        [XmlElement("Metadata")]
        public ScriptingEntityMetadata Metadata { get => _metadata; set => _metadata = value; }

        [XmlAttribute("Method")]
        public string Method { get => _method; set => _method = value; }

        [XmlElement("Arguments")]
        public ScriptingEntityArguments Arguments { get => _arguments; set => _arguments = value; }

        #endregion
    }
}
