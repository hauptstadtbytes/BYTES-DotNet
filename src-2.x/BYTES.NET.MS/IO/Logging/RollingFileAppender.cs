//import .net namespace(s) required
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BYTES.NET.IO.Logging;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.IO.Logging.API;
using BYTES.NET.IO;

//import namespace(s) required from 'log4net' library
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Layout;
using log4net.Core;

namespace BYTES.NET.MS.IO.Logging
{
    /// <summary>
    /// a rolling file appender
    /// </summary>
    /// <remarks>'Log4Net' wrapper class</remarks>
    public class RollingFileAppender : ILogAppender
    {

        #region private variable(s)

        private Log _parent = null;

        private string _filePath = null;
        private string _pattern = "%date %level [%thread] - %message%newline";
        private string _maxFileSize = "5MB";
        private int _maxBackupsCount = 3;
        private bool _dumpOnAttachement = true;

        private ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool _isAttached = false;

        #endregion

        #region public properties

        public string FilePath
        {
            get => _filePath;
            set
            {
                if (_isAttached)
                {
                    throw new InvalidOperationException("Unable to modify the file path after appender was attached");
                }

                _filePath = value;
            }
        }

        public string Pattern
        {
            get => _pattern;
            set
            {
                if (_isAttached)
                {
                    throw new InvalidOperationException("Unable to modify the pattern after appender was attached");
                }

                _pattern = value;
            }
        }

        public string MaxFileSize
        {
            get => _maxFileSize;
            set
            {
                if (_isAttached)
                {
                    throw new InvalidOperationException("Unable to modify the maximum file size after appender was attached");
                }

                _maxFileSize = value;
            }
        }

        public int MaxBackupsCount
        {
            get => _maxBackupsCount;
            set
            {
                if (_isAttached)
                {
                    throw new InvalidOperationException("Unable to modify the maximum number of backups after appender was attached");
                }

                _maxBackupsCount = value;
            }
        }

        public bool DumpOnAttachement
        {
            get => _dumpOnAttachement;
            set
            {
                if (_isAttached)
                {
                    throw new InvalidOperationException("Unable to modify the dumping behavior after appender was attached");
                }

                _dumpOnAttachement = value;
            }
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="args"></param>
        public RollingFileAppender(string filePath, Dictionary<string, object> args = null)
        {
            _filePath = filePath;

            if(args != null)
            {
                ParseArgs(args);
            }
        }

        #endregion

        #region public method(s) implementing 'ILogAppender'

        /// <summary>
        /// called on attachement
        /// </summary>
        /// <param name="log"></param>
        public void OnAppended(Log log)
        {
            _parent = log;

            //check for the output directory
            string dirPath = Path.GetDirectoryName(Helper.ExpandPath(_filePath));

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            //initialize the log
            InitializeLog4Net();
            _isAttached = true;

            //dump existing log entries
            if (_dumpOnAttachement)
            {
                _logger.Info("### start dumping log '" + _parent.ID + "' ###");

                foreach(LogEntry entry in _parent.GetCache())
                {
                    OnLogged(entry);
                }

                _logger.Info("### finished dumping log '" + _parent.ID + "' ###");
            }
        }

        /// <summary>
        /// called on logging
        /// </summary>
        /// <param name="entry"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnLogged(LogEntry entry)
        {
            if (entry.Level == LogEntry.InformationLevel.Debug)
            {
                _logger.Debug(entry.Message, (Exception)entry.Details);
            }
            else if (entry.Level == LogEntry.InformationLevel.Info)
            {
                _logger.Info(entry.Message, (Exception)entry.Details);
            }
            else if (entry.Level == LogEntry.InformationLevel.Warning)
            {
                _logger.Warn(entry.Message, (Exception)entry.Details);
            }
            else if (entry.Level == LogEntry.InformationLevel.Exception)
            {
                _logger.Error(entry.Message, (Exception)entry.Details);
            }
            else if (entry.Level == LogEntry.InformationLevel.Fatal)
            {
                _logger.Fatal(entry.Message, (Exception)entry.Details);
            }
        }

        #endregion

        #region private method(s)

        /// <summary>
        /// parses the argument(s) given to internal variable(s)
        /// </summary>
        /// <param name="args"></param>
        private void ParseArgs(Dictionary<string,object> args)
        {
            foreach(KeyValuePair<string,object> arg in args)
            {
                if(arg.Key.ToLower() == "pattern")
                {
                    _pattern = (string)arg.Value;
                } else if(arg.Key.ToLower() == "maxfilesize")
                {
                    _maxFileSize = (string)arg.Value;
                } else if(arg.Key.ToLower() == "maxbackupscount")
                {
                    _maxBackupsCount = (int)arg.Value;
                } else if(arg.Key.ToLower() == "dumponattachement")
                {
                    _dumpOnAttachement=(bool)arg.Value;
                }
            }
        }

        /// <summary>
        /// initializes the 'Log4Net' framework
        /// </summary>
        /// <remarks>based on the article found at 'https://stackoverflow.com/questions/16336917/can-you-configure-log4net-in-code-instead-of-using-a-config-file'</remarks>
        private void InitializeLog4Net()
        {
            //validate the argument(s)
            if (_filePath == null || String.IsNullOrEmpty(_filePath) || String.IsNullOrWhiteSpace(_filePath))
            {
                throw new ArgumentException("The file path must not be empty");
            }

            //check for (and create) the output folder
            string filePath = Helper.ExpandPath(_filePath);
            string rootPath = filePath.Substring(0, filePath.LastIndexOf("\\"));

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            _filePath = filePath;

            //create the layout
            PatternLayout layout = new PatternLayout() { ConversionPattern = _pattern };
            layout.ActivateOptions();

            //enable the rolling file appender
            log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();
            appender.Layout = layout;
            appender.AppendToFile = true;
            appender.File = _filePath;
            appender.StaticLogFileName = true;
            appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Size;
            appender.MaximumFileSize = _maxFileSize;
            appender.MaxSizeRollBackups = _maxBackupsCount;
            appender.ActivateOptions();

            //add the appender and configure the 'log4net' hierarchy
            Hierarchy hier = (Hierarchy)LogManager.GetRepository();
            hier.Root.AddAppender(appender);
            hier.Root.Level = Level.All;
            hier.Configured = true;
        }

        #endregion
    }
}
