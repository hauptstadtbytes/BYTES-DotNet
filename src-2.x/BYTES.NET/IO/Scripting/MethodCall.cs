﻿//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BYTES.NET.IO.Scripting
{
    [Serializable]
    public class MethodCall : ScriptingToken
    {
        #region private variable(s)

        private string _instance = Guid.NewGuid().ToString();

        private string _method = string.Empty;
        private MethodCallArguments _args = new MethodCallArguments();

        #endregion

        #region public properties

        [XmlAttribute(AttributeName = "ID")]
        public override string ID
        {
            get => _instance;
            set => _instance = value;
        }

        [XmlAttribute(AttributeName = "Method")]
        public string Method
        {
            get => _method;
            set => _method = value;
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