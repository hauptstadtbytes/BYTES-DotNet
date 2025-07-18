using System;
using System.ComponentModel;
using System.Windows.Input;
using BYTES.NET.WPF.MVVM;

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

        #endregion

        #region public properties

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string? Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
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
            set
            {
                if (_allowCancel != value)
                {
                    _allowCancel = value;
                    OnPropertyChanged();
                }
            }
        }

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

            // Add Cancel Command to Commands dictionary with shared relay command
            Commands.Add("CancelCmd", new ViewModelRelayCommand(
                _ => CancelRequested?.Invoke(),
                _ => AllowCancel
            ));
        }

        #endregion

        #region methods

        public void CloseDialog()
        {
            View?.Close();
        }

        #endregion
    }
}
