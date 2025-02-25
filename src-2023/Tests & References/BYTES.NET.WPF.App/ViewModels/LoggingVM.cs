    //import .net (default) namespace(s)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET.WPF' framework
using BYTES.NET.WPF.MVVM;
using BYTES.NET.Logging;

//import internal namespace(s) required
using BYTES.NET.WPF.App.Views;
using BYTES.NET.Logging.Appenders;
using System.Collections.ObjectModel;

//add namespace(s) required from Ookii framework
using Ookii.Dialogs.Wpf;

namespace BYTES.NET.WPF.App.ViewModels
{
    public class LoggingVM : ViewModel
    {
        #region private variable(s)

        private LoggingView _view = new LoggingView();

        private string _msgText = String.Empty;
        private LogEntry.InformationLevel _msgInformationLevel = LogEntry.InformationLevel.Info;

        private Log _log = new Log();

        private string _filePath;
        private PlainTextAppender? _plainTextAppender = null;

        #endregion

        #region public properties

        public LoggingView View { get => _view; }

        public Array InformationLevels => Enum.GetValues(typeof(LogEntry.InformationLevel));

        public string MessageText
        {
            get => _msgText; 
            
            set
            {
                _msgText = value;
                OnPropertyChanged();
            }
        }

        public LogEntry.InformationLevel MessageInformationLevel
        {
            get => _msgInformationLevel;
            set
            {
                _msgInformationLevel = value;
                OnPropertyChanged();
            }
        }

        public LogEntry[] LogEntries
        {
            get => _log.Cache.ToArray();

        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        public LoggingVM()
        {
            //initialize the view
            _view.DataContext = this;

            //add the welcome message
            _log.Inform("Please enter a new message and click 'Write'. Currently there is no log file appended. Please use the respective button to select a file path.");

            //add logging command (s)
            this.Commands.Add("LogMessage", new ViewModelRelayCommand(LogMessage));
            this.Commands.Add("SelectFilePathCmd", new ViewModelRelayCommand(SelectFilePath));
        }

        #endregion

        #region private method(s) for logging example(s)

        /// <summary>
        /// logs a message
        /// </summary>
        /// <param name="arg"></param>
        private void LogMessage(object arg)
        {

            if (string.IsNullOrEmpty(MessageText))
            {
                _log.Write("Error: Your message must not be empty",LogEntry.InformationLevel.Warning);

            } else
            {
                //write the message to the log
                LogEntry entry = new LogEntry(MessageText, MessageInformationLevel);
                _log.Write(entry);
            }

            //clear the message input
            MessageText = String.Empty;

            //update the view
            OnPropertyChanged("LogEntries");

        }

        /// <summary>
        /// appends a new log file
        /// </summary>
        private void SelectFilePath() {

            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Ordner auswählen";
            dialog.UseDescriptionForTitle = true;
            dialog.ShowNewFolderButton = false;
            dialog.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                //removes all existing appenders
                _log.ClearAppenders();

                //adds a new play text appender
                PlainTextAppender appender = new PlainTextAppender(dialog.SelectedPath, "SampleLogFile");
                _log.AddAppender(appender);

                //adds a message to the log
                _log.Inform("Text file appended for logging at '" + appender.FullPath + "'");
                OnPropertyChanged("LogEntries");
            }

        }

        #endregion

    }
}
