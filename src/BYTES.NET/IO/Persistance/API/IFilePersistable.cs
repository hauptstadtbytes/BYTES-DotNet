//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.Persistance.API
{
    public interface IFilePersistable
    {
        /// <summary>
        /// loads data from an existing 'IPersistable' instance
        /// </summary>
        /// <param name="data"></param>
        void FromIPersistable(IFilePersistable data);

        /// <summary>
        /// converts an instance to a data table
        /// </summary>
        /// <returns></returns>
        DataTable ToTable();

        /// <summary>
        /// loads data from an existing table
        /// </summary>
        /// <param name="data"></param>
        void FromTable(DataTable data);

    }
}
