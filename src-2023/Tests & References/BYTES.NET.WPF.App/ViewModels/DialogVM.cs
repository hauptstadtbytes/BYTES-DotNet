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
using BYTES.NET.WPF.App.Views;
using BYTES.NET.WPF.MVVM;

namespace BYTES.NET.WPF.App.ViewModels
{
    public class DialogVM : DialogViewModel
    {
        public ICommand CloseViewCommand { get; }
        /// <summary>
        /// Event that is raised when the user submits the text
        /// </summary>
        public event EventHandler<string> MessageUpdated;
        /// <summary>
        /// bool to determine if the dialog is blocking
        /// </summary>
        public bool isBlocking { get; set; }

        /// <summary>
        /// String to hold the dialog message
        /// </summary>
        private string _dialogMessage;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_isBlocking"></param>
        public DialogVM()
        {
            CloseViewCommand = new ViewModelRelayCommand(CloseView);
        }

        public DialogVM(Window MyView)
        {
            CloseViewCommand = new ViewModelRelayCommand(CloseView);
            _myView = MyView;
            
        }

        // Method to close the view
        private void CloseView()
        {
            _myView?.Close();
        }


        /// <summary>
        /// string to hold the dialog message
        /// </summary>
        public string DialogMessage
        {
            get => _dialogMessage;
            set
            {
                 _dialogMessage = value;
                 OnPropertyChanged();
                 MessageUpdated?.Invoke(this, _dialogMessage);
            }
        }
    }
}
