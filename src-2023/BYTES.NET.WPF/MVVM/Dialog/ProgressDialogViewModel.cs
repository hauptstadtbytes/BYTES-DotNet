using System;
using System.Collections.ObjectModel;
using System.Linq;
using BYTES.NET.Logging;
using BYTES.NET.Logging.Appenders;
using BYTES.NET.WPF.MVVM;
using Ookii.Dialogs.Wpf;

namespace BYTES.NET.WPF.MVVM.Dialog
{
    public class ProgressDialogViewModel : DialogViewModel<ProgressDialogView>
    {
        #region private fields

        private string _title = string.Empty;
        private string? _message = null;
        private double? _total = null;
        private double _current = 0;
        private bool _allowCancel = true;

        private readonly Log _log = new Log();
        private PlainTextAppender? _plainTextAppender = null;
        private ObservableCollection<LogEntry> _logCollection = new ObservableCollection<LogEntry>();
        private bool _hasLogEntries;
        private string _logText = string.Empty;
        private bool _isLogExpanded = false;

        #endregion

        #region public properties

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string? Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        public double? Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsIndeterminate));
                OnPropertyChanged(nameof(ProgressValue));
            }
        }

        public double Current
        {
            get => _current;
            set
            {
                _current = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProgressValue));
            }
        }

        public bool IsIndeterminate => !_total.HasValue || _total == 0;
        public double ProgressValue => IsIndeterminate ? 0 : (_current / _total.Value) * 100;

        public bool AllowCancel
        {
            get => _allowCancel;
            set { _allowCancel = value; OnPropertyChanged(); }
        }

        public ObservableCollection<LogEntry> LogCollection { get; private set; } = new ObservableCollection<LogEntry>();

        public bool HasLogEntries
        {
            get => _hasLogEntries;
            private set
            {
                if (_hasLogEntries != value)
                {
                    _hasLogEntries = value;
                    OnPropertyChanged(nameof(HasLogEntries));
                    OnPropertyChanged(nameof(HasLogsAndExpanded)); // notify combined property
                }
            }
        }

        public string LogText
        {
            get => _logText;
            private set
            {
                if (_logText != value)
                {
                    _logText = value;
                    OnPropertyChanged(nameof(LogText));
                }
            }
        }

        public bool IsLogExpanded
        {
            get => _isLogExpanded;
            set
            {
                if (_isLogExpanded != value)
                {
                    _isLogExpanded = value;
                    OnPropertyChanged(nameof(IsLogExpanded));
                    OnPropertyChanged(nameof(HasLogsAndExpanded)); // notify combined property
                }
            }
        }

        public bool HasLogsAndExpanded => HasLogEntries && IsLogExpanded;
        #endregion

        #region events

        public event Action? CancelRequested;

        #endregion

        #region constructor

        public ProgressDialogViewModel(string title, string? message = null)
        {
            Title = title;
            Message = message;
            View = new ProgressDialogView();

            Commands.Add("CancelCmd", new ViewModelRelayCommand(_ => CancelRequested?.Invoke(), _ => AllowCancel));
            Commands.Add("SelectFilePathCmd", new ViewModelRelayCommand(_ => SelectFilePath()));
            Commands.Add("ToggleLogCmd", new ViewModelRelayCommand(_ => IsLogExpanded = !IsLogExpanded));

            _log.Inform($"Progress Dialog '{Title}' initialized.");

            LogCollection.CollectionChanged += (s, e) =>
            {
                HasLogEntries = LogCollection.Count > 0;
                LogText = string.Join(Environment.NewLine, LogCollection.Select(l => l.Message));
            };
        }

        #endregion

        #region public logging helpers

        public void LogInfo(string message)
        {
            _log.Write(message, LogEntry.InformationLevel.Info);
            LogCollection.Add(new LogEntry(Message = message, LogEntry.InformationLevel.Info));
        }

        public void LogWarning(string message)
        {
            _log.Write(message, LogEntry.InformationLevel.Warning);
            LogCollection.Add(new LogEntry(Message = message, LogEntry.InformationLevel.Warning));
        }

        public void LogError(string message)
        {
            _log.Write(message, LogEntry.InformationLevel.Exception);
            LogCollection.Add(new LogEntry(Message = message, LogEntry.InformationLevel.Exception));
        }

        #endregion

        #region private methods

        private void SelectFilePath()
        {
            var dialog = new VistaFolderBrowserDialog
            {
                Description = "Select log folder",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = false
            };

            if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                _log.ClearAppenders();
                _plainTextAppender = new PlainTextAppender(dialog.SelectedPath, "ProgressDialogLog");
                _log.AddAppender(_plainTextAppender);

                _log.Inform($"File logging enabled at '{_plainTextAppender.FullPath}'");
                OnPropertyChanged(nameof(LogCollection));
            }
        }

        public void CloseDialog()
        {
            View?.Close();
        }

        #endregion
    }
}
