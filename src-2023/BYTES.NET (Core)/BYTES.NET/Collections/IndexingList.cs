//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BYTES.NET.Collections
{
    /// <summary>
    /// a generic list, supporting (self-)indexing
    /// </summary>
    [Serializable]
    public class IndexingList<TType, TIndex> : List<TType>
    {
        #region protected variable(s)

        protected Func<TType, TIndex> _getIndexCallback = null;
        protected Func<TType, TIndex, bool> _validateIndexCallback = null;

        #endregion

        #region public properties

        [XmlIgnore]
        public TIndex[] Indices
        {
            get
            {
                List<TIndex> output = new List<TIndex>();

                foreach (TType item in this)
                {
                    TIndex index = _getIndexCallback(item);

                    if (!output.Contains(index))
                    {
                        output.Add(index);
                    }
                   
                }

                output.Sort();

                return output.ToArray();
            }
        }

        [XmlIgnore]
        public TType[] this[TIndex index]
        {
            get
            {
                List<TType> output = new List<TType>();

                foreach (TType item in this)
                {
                    if (_validateIndexCallback(item, index))
                    {
                        output.Add(item);
                    }
                }

                return output.ToArray();
            }
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="getIndexCallback"></param>
        /// <param name="validateIndexCallback"></param>
        public IndexingList(Func<TType, TIndex> getIndexCallback, Func<TType, TIndex, bool> validateIndexCallback) : base()
        {
            _getIndexCallback = getIndexCallback;
            _validateIndexCallback = validateIndexCallback;
        }

        #endregion
    }
}
