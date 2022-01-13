//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BYTES.NET.IO.Scripting
{
    [Serializable]
    public abstract class ScriptingToken
    {
        #region public properties

        [XmlAttribute(AttributeName = "ID")]
        public virtual string ID { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        #endregion
    }
}
