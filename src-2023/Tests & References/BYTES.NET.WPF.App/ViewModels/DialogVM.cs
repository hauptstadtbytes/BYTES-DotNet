//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

//import namespace(s) required from BYTES.NET.WPF framework
using BYTES.NET.WPF.MVVM;

//import internal namespace(s) required
using BYTES.NET.WPF.App.Views;

namespace BYTES.NET.WPF.App.ViewModels
{
    public class DialogVM : DialogViewModel<DialogView>
    {

        #region private variable(s)

        private string _dialogMessage;

        #endregion

        #region public properties

        public string DialogMessage
        {
            get => _dialogMessage;
            set
            {
                _dialogMessage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// the constructor instance
        /// </summary>
        public DialogVM()
        {
        }

        #endregion
        
    }
}
