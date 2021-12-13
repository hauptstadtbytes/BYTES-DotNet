using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.Logging.API
{
    /// <summary>
    /// a generic log appender interface
    /// </summary>
    public interface ILogAppender
    {
        /// <summary>
        /// called after adding the appender to a log
        /// </summary>
        /// <param name="log"></param>
        void OnAppended(Log log);

        /// <summary>
        /// called after adding a new log entry to the parent log
        /// </summary>
        /// <param name="entry"></param>
        void OnLogged(LogEntry entry);
    }
}
