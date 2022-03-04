//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BYTES.NET.Collections
{
    [Serializable]
    public abstract class ExtendedList<TType,TIndex> : List<TType>
    {
        #region protected variable(s)

        protected Func<TType, TIndex>? _getIndexCallback = null;
        protected Func<TType,TIndex, bool>? _validateIndexCallback = null;

        #endregion

        #region public properties

        [XmlIgnore]
        public TIndex[] Indices
        {
            get
            {
                List<TIndex> output = new List<TIndex>();

                if(_getIndexCallback != null)
                {
                    foreach (TType item in this)
                    {
                        output.Add(_getIndexCallback(item));
                    }
                }

                return output.ToArray();
            }
        }

        [XmlIgnore]
        public TType? this[TIndex index]
        {
            get
            {
                if(_validateIndexCallback != null)
                {
                    foreach (TType item in this)
                    {
                        if (_validateIndexCallback(item,index))
                        {
                            return item;
                        }
                    }
                }

                return default(TType);
            }
        }

        #endregion

    }
}
