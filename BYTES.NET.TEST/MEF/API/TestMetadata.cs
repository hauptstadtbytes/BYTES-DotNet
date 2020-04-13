//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' library
using BYTES.NET.MEF.API;

namespace BYTES.NET.TEST.MEF.API
{
    class TestMetadata : Metadata
    {

        public TestMetadata() : base(typeof(ITestInterface))
        {
        }

    }

}
