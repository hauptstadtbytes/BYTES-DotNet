using System;
using System.Collections.Generic;
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

        private string? _message = null;
        private double? _total = null;
        private double _current = 0;
        private bool _allowCancel = true;

        private Log _log = new Log();
        private bool _detailsExpanded = false;

        #endregion

        #region public properties

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
                OnPropertyChanged("IsIndeterminate");
            }
        }

        public double Current
        {
            get => _current;
            set
            {
                _current = value;
                OnPropertyChanged();
            }
        }

        public bool IsIndeterminate { get => !_total.HasValue || _total == 0; }
        
        public bool AllowCancel
        {
            get => _allowCancel;
            set { _allowCancel = value; OnPropertyChanged(); }
        }

        public Log Log { get => _log; set {
                _log = value;
                _log.Logged += HandleLogLogged;
                OnPropertyChanged();
            } 
        }

        public List<LogEntry> LogCache { get => this.Log.Cache; }

        public bool IsDetailsExpanded { get => _detailsExpanded; set
            {
                _detailsExpanded = value;
                OnPropertyChanged();
            } }

        #endregion

        #region events

        public event Action? CancelRequested;

        #endregion

        #region constructor

        public ProgressDialogViewModel(string? message = null)
        {

            Message = message;
            View = new ProgressDialogView();

            this.Log = new Log();

            Commands.Add("CancelCmd", new ViewModelRelayCommand(_ => CancelRequested?.Invoke(), _ => AllowCancel));

        }

        #endregion

        #region private methods

        public void HandleLogLogged(ref LogEntry entry)
        {
            OnPropertyChanged("Log");
            OnPropertyChanged("LogCache");
        }

        public void CloseDialog()
        {
            View?.Close();
        }

        #endregion
    }
}
