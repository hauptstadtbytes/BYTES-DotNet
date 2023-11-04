//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Logging.API
{
    public interface ILogAppender
    {
        #region method(s)

        /// <summary>
        /// called directly after adding the appender to a log
        /// </summary>
        /// <param name="log"></param>
        void OnAppended(Log log);

        /// <summary>
        /// called after adding a new log entry
        /// </summary>
        /// <param name="entry"></param>
        void OnLogged(LogEntry entry);

        #endregion
    }
}
