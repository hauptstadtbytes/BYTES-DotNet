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
    public class Sequence : ScriptingToken 
    {
        #region private variable(s)

        private string _instance = Guid.NewGuid().ToString();

        private List<MethodCall> _calls = new List<MethodCall>();

        #endregion

        #region public properties

        [XmlAttribute(AttributeName = "ID")]
        public override string ID
        {
            get => _instance;
            set => _instance = value;
        }

        [XmlArrayItem("Call")]
        public List<MethodCall> Calls
        {
            get => _calls;
            set => _calls = value;
        }

        #endregion
    }
}
