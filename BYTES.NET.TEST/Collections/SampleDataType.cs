//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.TEST.Collections
{
    [Serializable]
    public class SampleDataType
    {

        #region public properties

        public string Description { get; set; }
        public int Counter { get; set; }

        #endregion

        #region public new instance method(s)

        public SampleDataType()
        {

            this.Counter = 42;
            this.Description = "Lorem ipsum...";

        }

        #endregion

    }

}
