//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

//import internal namespace(s) required
using BYTES.NET.IO.Persistance.API;

namespace BYTES.NET.IO.Scripting
{
    [Serializable]
    public class Script : IFilePersistable 
    {
        #region private variable(s)

        private string _instanceID = Guid.NewGuid().ToString();

        private List<MethodCall> _sequence = new List<MethodCall>();

        #endregion

        #region public properties

        [XmlAttribute(AttributeName = "Instance")]
        public string InstanceID
        {
            get => _instanceID;
            set => _instanceID = value;
        }

        [XmlArrayItem("Call")]
        public List<MethodCall> Sequence
        {
            get => _sequence;
            set => _sequence = value;
        }

        #endregion

        #region public method(s) implementing 'IFilePersistable'

        /// <summary>
        /// loads the data from a 'IFilePersistable' instance
        /// </summary>
        /// <param name="data"></param>
        public void Load(IFilePersistable data)
        {
            Script updated = (Script)data;

            this.InstanceID = updated.InstanceID;
            this.Sequence = updated.Sequence;
        }

        #endregion
    }
}
