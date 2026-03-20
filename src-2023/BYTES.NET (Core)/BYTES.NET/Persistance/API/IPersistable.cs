//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Persistance.API
{
    public interface IPersistable
    {
        /// <summary>
        /// updates the instance data from an 'IPersistable' instance
        /// </summary>
        /// <param name="data"></param>
        void UpdateFromIPersistable(IPersistable data);
    }
}
