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
    public class MethodCall
    {
        #region private variable(s)

        private string _instanceID = Guid.NewGuid().ToString();

        private string _methodID = string.Empty;
        private MethodCallArguments _args = new MethodCallArguments();

        #endregion

        #region public properties

        [XmlAttribute(AttributeName = "Method")]
        public string MethodID
        {
            get => _methodID;
            set => _methodID = value;
        }

        [XmlAttribute(AttributeName = "Instance")]
        public string InstanceID
        {
            get => _instanceID;
            set => _instanceID = value;
        }

        [XmlElement(ElementName = "Arguments")]
        public MethodCallArguments Arguments
        {
            get => _args;
            set => _args = value;
        }

        #endregion
    }
}
