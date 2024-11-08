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
    [XmlRoot("Metadata")]
    public class ScriptingEntityMetadata
    {
        #region private variable(s)

        private string _name = string.Empty;
        private string _description = string.Empty;

        #endregion

        #region public properties

        [XmlElement("Name")]
        public string Name { get => _name; set => _name = value; }

        [XmlElement("Description")]
        public string Description { get => _description; set => _description = value; }

        #endregion
    }
}
