using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Collections;

namespace BYTES.NET.Test.Collections
{
    public class ListDictionary : ExtendedDictionary<int,string>
    {

        #region protected properties

        protected override bool Serialize => false;

        protected override string XmlItemName => "ListItem";
        protected override string XmlKeyName => "Counter";
        protected override string XmlValueName => "Content";

        protected override bool EmbedStructure => false;

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        public ListDictionary() : base()
        {
        }

        #endregion

    }
}
