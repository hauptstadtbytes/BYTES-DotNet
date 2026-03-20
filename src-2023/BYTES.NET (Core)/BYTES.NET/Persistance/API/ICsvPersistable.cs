//import .net (default) namespace(s) required
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Persistance.API
{
    public interface ICsvPersistable : IPersistable
    {
        /// <summary>
        /// returns a 'DataTable' type representing the instance data
        /// </summary>
        /// <returns></returns>
        public DataTable ToDataTable();

        /// <summary>
        /// updates the instance data from an 'DataTable' instance
        /// </summary>
        /// <param name="data"></param>
        public void UpdateFromDataTable(DataTable data);
    }
}
