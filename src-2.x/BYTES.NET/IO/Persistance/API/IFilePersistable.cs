//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.Persistance.API
{
    public interface IFilePersistable
    {
        /// <summary>
        /// loads data from given 'IPersistable' instance
        /// </summary>
        /// <param name="data"></param>
        void Load(IFilePersistable data);
    }
}
