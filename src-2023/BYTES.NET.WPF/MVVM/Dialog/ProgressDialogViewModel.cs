using System;
using System.ComponentModel;

namespace BYTES.NET.WPF.MVVM.Dialog
{
    /// <summary>
    /// ViewModel for Progress Dialog supporting title, message, and progress values.
    /// </summary>
    public class ProgressDialogViewModel : DialogViewModel<ProgressDialogView>
    {
        #region private fields

        private string _title = string.Empty;
        private string? _message = null;

        private double? _total = null;
        private double _current = 0;

        #endregion

        #region public properties

        /// <summary>
        /// Title displayed in the dialog.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Optional message displayed below the title.
        /// </summary>
        public string? Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Total amount of work for progress calculation. If null or zero, infinite progress is shown.
        /// </summary>
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

        /// <summary>
        /// Current progress value.
        /// </summary>
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

        /// <summary>
        /// Returns true if Total is not set or zero, meaning the progress is indeterminate.
        /// </summary>
        public bool IsIndeterminate => !_total.HasValue || _total == 0;

        /// <summary>
        /// Returns the progress percentage (0–100), used for determinate progress bars.
        /// </summary>
        public double ProgressValue => IsIndeterminate ? 0 : (_current / _total.Value) * 100;

        #endregion

        #region constructor

        /// <summary>
        /// Constructor for ProgressDialogViewModel.
        /// </summary>
        /// <param name="title">The title to display.</param>
        /// <param name="message">Optional message to display.</param>
        public ProgressDialogViewModel(string title, string? message = null)
        {
            Title = title;
            Message = message;

            // Initialize the view
            View = new ProgressDialogView();
        }

        #endregion
    }
}
