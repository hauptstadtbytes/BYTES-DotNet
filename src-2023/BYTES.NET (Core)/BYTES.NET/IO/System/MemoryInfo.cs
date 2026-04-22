using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.System
{
    /// <summary>
    /// Class to format Memory 
    /// </summary>
    public class MemoryInfo
    {
        #region private properties

        private readonly ulong _bytes;

        #endregion


        #region public properties

        public double InBytes => _bytes;
        public double InKB => _bytes / 1024.0;
        public double InMB => _bytes / 1024.0 / 1024.0;
        public double InGB => _bytes / 1024.0 / 1024.0 / 1024.0;
        public double InTB => _bytes / 1024.0 / 1024.0 / 1024.0 / 1024.0;

        #endregion


        #region constructor

        public MemoryInfo(ulong bytes)
        {
            _bytes = bytes;
        }

        #endregion

    }
}
