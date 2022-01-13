//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.IO.Logging;

namespace BYTES.NET.IO
{
    public abstract class ContextBase
    {
        #region public event(s)

        public event MessageReceivedEventhandler MessageReceived;
        public delegate void MessageReceivedEventhandler(ref LogEntry message);

        #endregion

        #region public method(s)

        /// <summary>
        /// writes a message
        /// </summary>
        /// <param name="entry"></param>
        public void WriteMessage(LogEntry entry)
        {
            OnMessageReceived(entry);
        }

        /// <summary>
        /// writes a message from string
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="details"></param>
        public void WriteMessage(string message, LogEntry.InformationLevel level = LogEntry.InformationLevel.Info, object? details = null)
        {
            if (details == null)
            {
                WriteMessage(new LogEntry(message, level));
            }
            else
            {
                WriteMessage(new LogEntry(message, level, details));
            }
        }

        /// <summary>
        /// adds a 'Debug' level entry
        /// </summary>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public void Trace(string message, object? details = null)
        {
            WriteMessage(message, LogEntry.InformationLevel.Debug, details);
        }

        /// <summary>
        /// adds a 'Info' level entry
        /// </summary>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public void Inform(string message, object? details = null)
        {
            WriteMessage(message, LogEntry.InformationLevel.Info, details);
        }

        /// <summary>
        /// adds a 'Warning' level entry
        /// </summary>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public void Warn(string message, object? details = null)
        {
            WriteMessage(message, LogEntry.InformationLevel.Warning, details);
        }

        /// <summary>
        /// adds a (fatal) error level entry
        /// </summary>
        /// <param name="message"></param>
        /// <param name="details"></param>
        /// <param name="isFatal"></param>
        public void ReportError(string message, object? details = null, bool isFatal = false)
        {
            if (isFatal)
            {
                WriteMessage(message, LogEntry.InformationLevel.Fatal, details);
            }
            else
            {
                WriteMessage(message, LogEntry.InformationLevel.Exception, details);
            }
        }

        #endregion

        #region protected method(s)

        /// <summary>
        /// raises the 'MessageReceived' event
        /// </summary>
        /// <param name="message"></param>
        protected virtual void OnMessageReceived(LogEntry message)
        {
            if (MessageReceived != null)
            {
                MessageReceived(ref message);
            }
        }

        #endregion

    }
}
