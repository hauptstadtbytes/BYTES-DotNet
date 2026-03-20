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

        private string _filePath;
        private string _fileName;

        #endregion

        #region public variable(s)

        public string FullPath
        {
            get => Path.Combine(_filePath, $"{_fileName}.txt");
        }

        #endregion

        #region constructor(s)/ public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PlainTextAppender(string filePath, string fileName)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _fileName = fileName;
        }
        #endregion

        #region public method(s) implementing 'ILogAppender'

        public void OnAppended(Log log)
        {
            //CreateLogFile(log);
        }

        public void OnLogged(LogEntry entry)
        {
            WriteToLogFile(entry);
        }

        #endregion

        #region private method(s)

        /// <summary>
        /// creates a new text file (on disk)
        /// </summary>
        /// <param name="log"></param>
        private void CreateLogFile(Log log)
        {
            if (!File.Exists(this.FullPath))
            {
                File.Create(this.FullPath);
            }
        }

        /// <summary>
        /// writes a message to log file
        /// </summary>
        /// <param name="entry"></param>
        private void WriteToLogFile(LogEntry entry)
        {
            string logMessage = $"{DateTime.Now}; {entry.Message}; Informationlevel; {entry.Level}\n";

            File.AppendAllText(this.FullPath, logMessage);
        }


        #endregion
    }
}
