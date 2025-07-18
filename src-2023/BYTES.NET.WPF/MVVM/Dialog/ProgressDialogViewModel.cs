//import .net (default) namespace(s)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.WPF.MVVM.Dialog
{
    public class ProgressDialogViewModel: DialogViewModel<ProgressDialogView>
    {

        #region private variable(s)

        private string _title = String.Empty;
        private string? _message = null;

        #endregion

        #region public properties(s)

        public string Title
        {
            get => _title; set
            {
                _title = value;

                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => _message; set
            {
                _message = value;

                OnPropertyChanged();
            }
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// the default constructor
        /// </summary>
        public ProgressDialogViewModel(string title, string message = null)
        {
            //set the properties
            this.Title = title;

            if (message != null)
            {
                this.Message = message;
            }

            //create the view
            this.View = new ProgressDialogView();

        }

        #endregion

    }
}
