﻿//import .NET namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;


//import namespace(s) requried from 'BYTES.NET' framework
using BYTES.NET.Persistance.API;
;

namespace BYTES.NET.Automation.Scripting
{
    /// <summary>
    /// the (automation) script (base) class
    /// </summary>
    [Serializable]
    public class Script : IXmlPersistable
    {
        #region private variable(s)

        protected Guid _id = Guid.NewGuid();
        protected string _name = "A New Automation Script";
        protected string _description = string.Empty;

        #endregion

        #region public properties

        /// <summary>
        /// default new instance method
        /// </summary>
        public Script()
        {
        }

        #endregion

        #region public method(s) implementing 'IXmlPersistable'

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void UpdateFromIPersistable(IPersistable data)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("metadata");
        }

        #endregion
    }
}
