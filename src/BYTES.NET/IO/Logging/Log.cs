//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.IO.Logging.API;

namespace BYTES.NET.IO.Logging
{
    /// <summary>
    /// generic log class
    /// </summary>
    public class Log
    {
        #region public event(s)

        public event LoggedEventHandler Logged;
        public delegate void LoggedEventHandler(ref LogEntry entry);

        #endregion

        #region private variable(s)

        private string _id = Guid.NewGuid().ToString();

        private List<LogEntry> _cache = new List<LogEntry>();
        private int _cacheLimit = 100;

        private LogEntry.InformationLevel _threshold = LogEntry.InformationLevel.Info;

        private List<ILogAppender> _appenders = new List<ILogAppender>();

        #endregion

        #region public properties

        public string ID
        {
            get => _id;
            set => _id = value;
        }

        public int CacheLimit
        {
            get => _cacheLimit;
            set => _cacheLimit = value;
        }

        public LogEntry.InformationLevel Threshold
        {
            get => _threshold;
            set => _threshold = value;
        }

        public ILogAppender[] Appenders
        {
            get => _appenders.ToArray();
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="identifyer"></param>
        public Log(string? identifyer = null)
        {
            if(identifyer != null)
            {
                _id = identifyer;
            }
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// add a new appender
        /// </summary>
        /// <param name="appender"></param>
        public void AddAppender(ILogAppender appender)
        {
            //add the appender
            _appenders.Add(appender);

            //call the 'OnAppended' method
            Log me = this;
            appender.OnAppended(me);
        }

        /// <summary>
        /// adds a new log entry
        /// </summary>
        /// <param name="entry"></param>
        public void AddEntry(LogEntry entry)
        {
            //add the entry to the local cache
            if (entry.MatchesThreshold(_threshold))
            {
                if(_cacheLimit > 0 || _cacheLimit == -1) //add the entry only for infinite caching and/ or a given limit
                {
                    _cache.Add(entry);

                    //cleanup the cache, if required
                    if(_cacheLimit > 0 && _cache.Count > 0)
                    {
                        while (_cache.Count > _cacheLimit)
                        {
                            _cache.RemoveAt(0);
                        }
                    }
                    
                }

                //route the new entry to the appenders (registrated)
                foreach (ILogAppender appender in _appenders)
                {
                    appender.OnLogged(entry);
                }

                //raise the 'Logged' event
                OnLogged(ref entry);
            }
        }

        /// <summary>
        /// adds a new log entry by details given
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="details"></param>
        public void AddEntry(string message, LogEntry.InformationLevel level, object? details = null)
        {
            if(details == null)
            {
                AddEntry(new LogEntry(message, level));
            }
            else
            {
                AddEntry(new LogEntry(message,level,details));
            }
        }

        /// <summary>
        /// adds a 'Debug' level entry
        /// </summary>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public void Trace(string message, object? details = null)
        {
            AddEntry(message,LogEntry.InformationLevel.Debug,details);
        }

        /// <summary>
        /// adds a 'Info' level entry
        /// </summary>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public void Inform(string message, object? details = null)
        {
            AddEntry(message, LogEntry.InformationLevel.Info, details);
        }

        /// <summary>
        /// adds a 'Warning' level entry
        /// </summary>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public void Warn(string message, object? details = null)
        {
            AddEntry(message, LogEntry.InformationLevel.Warning, details);
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
                AddEntry(message, LogEntry.InformationLevel.Fatal, details);
            } else
            {
                AddEntry(message, LogEntry.InformationLevel.Exception, details);
            }  
        }

        /// <summary>
        /// returns the chached log entrie(s)
        /// </summary>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public LogEntry[] GetCache(LogEntry.InformationLevel? threshold = null)
        {

            if (threshold.HasValue)
            {
                List<LogEntry> output = new List<LogEntry>();

                foreach(LogEntry entry in _cache)
                {
                    if (entry.MatchesThreshold((LogEntry.InformationLevel)threshold))
                    {
                        output.Add(entry);
                    }
                }

                return output.ToArray();
            }

            return _cache.ToArray();

        }

        #endregion

        #region protected method(s)

        /// <summary>
        /// raises the 'Logged' event
        /// </summary>
        /// <param name="entry"></param>
        protected virtual void OnLogged(ref LogEntry entry)
        {
            if (Logged != null)
            {
                Logged(ref entry);
            }
        }

        #endregion

    }
}
