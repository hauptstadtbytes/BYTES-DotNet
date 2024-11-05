using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BYTES.NET.Logging.API;

namespace BYTES.NET.Logging.Appenders
{
    public class PlainTextAppender : ILogAppender
    {
        #region private variable(s)

        private string _logFilePath;

        private string _logFile;

        #endregion

        #region public variable(s)

        public string LogFilePath
        {
            get => _logFilePath;
            set
            {
                _logFilePath = value;
            }
        }

        #endregion

        #region ILogAppender implementation
        public void OnAppended(Log log)
        {
            CreateLogFile(log);
        }

        public void OnLogged(LogEntry entry)
        {
            WriteToLogFile(entry);
        }
        #endregion

        #region private method(s)

        private void CreateLogFile(object logObject)
        {
            if (!File.Exists(_logFilePath + _logFile))
            {
                File.Create(_logFilePath + _logFile + ".txt");
            }
        }

        private void WriteToLogFile(LogEntry entry)
        {
            string fullPath = Path.Combine(_logFilePath, $"{_logFile}.txt");
            string logMessage = $"{DateTime.Now}: {entry.Message} Informationlevel: {entry.Level}\n";

            try
            {
                File.AppendAllText(fullPath, logMessage);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Error writing to log file: {ex.Message}", "Log Error", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Log Error", MessageBoxButton.OK);
            }
        }


        #endregion

        #region constructor(s)
        public PlainTextAppender(string logFilePath, string logFile)
        {
            _logFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
            _logFile = logFile;
        }
        #endregion
    }
}
