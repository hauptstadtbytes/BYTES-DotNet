using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BYTES.NET.WPF.MVVM;

namespace BYTES.NET.WPF.App.ViewModels
{
    public class MainVM : ViewModel
    {
        #region private variable(s)

        private string _title = "WPF Sample Application";

        #endregion

        #region public properties

        public string Title
        {
            get => _title; set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        public MainVM()
        {
            this.Commands.Add("PromptTextCmd", new ViewModelRelayCommand(PromptText));
        }

        #endregion

        #region private method(s)

        private void PromptText(object arg)
        {
            MessageBox.Show((string)arg);
        }

        #endregion

    }
}
