//import .net (default) namespace(s)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace BYTES.NET.WPF.MVVM
{
    ///<summary>
    ///dialog viemodel base class
    ///</summary>
    public abstract class DialogViewModel<TView> : ViewModel where TView : Window
    {
        #region private variable(s)

        private bool _isBlocking = false;

        private Window? _myView = null;

        #endregion

        #region private variable(s), required for non-blocking dialogs

        private Thread _myThread;

        ///<remarks>see 'https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads' for additional details</remarks>
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        #endregion

        #region public event(s), required for closing the dialog

        public delegate void DialogClosedEventHandler(object source);

        public event DialogClosedEventHandler DialogClosed;

        #endregion

        #region public properties

        public bool IsBlocking { get => _isBlocking; }

        public TView View
        {
            get
            {
                if (_myView == null)
                {
                    _myView = (TView)Activator.CreateInstance(typeof(TView));

                    _myView.DataContext = this;
                    _myView.Closed += OnClosed;
                }

                return (TView)_myView;
            }
            set
            {
                _myView = value;

                _myView.DataContext = this;
                _myView.Closed += OnClosed;

                OnPropertyChanged();
            }
        }

        public int ThreadID { get => _myThread.ManagedThreadId; }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// the default constructor
        /// </summary>
        public DialogViewModel()
        {
            //create a (default) command for closing the dialog
            this.Commands.Add("CloseDialogCmd", new ViewModelRelayCommand(CloseDialog));
        }

        #endregion

        #region public method(s) for opening/ closing the dialog

        /// <summary>
        /// showing/ opening the dialog (view)
        /// </summary>
        /// <param name="isBlocking"></param>
        public void ShowDialog(bool isBlocking)
        {
            this._isBlocking = isBlocking; //set the blocking value
            OnPropertyChanged("IsBlocking");

            //show the view
            if (this._isBlocking)
            {
                this.View.ShowDialog(); //use the default (blocking) mechanism
            }
            else
            {
                ShowDialogNonBlocking(); //use the custom (non-blocking) mechanism
            }
        }

        /// <summary>
        /// closing the dialog
        /// </summary>
        public void CloseDialog()
        {
            //close the view
            View.Close();

            //perform additional operations for non-blocking dialogs
            if (!this._isBlocking)
            {
                CloseNonBlocking();
            }

        }

        #endregion

        #region private method(s) for showing/ closing the dialog

        /// <summary>
        /// method showing up the dialog in a non-blocking mode
        /// </summary>
        private void ShowDialogNonBlocking()
        {
            //clean-up the environment
            if (_myThread != null)
            {
                _cancellationTokenSource.Cancel(); // cancel the previous token
                _myThread.Join(); // wait for the previous thread to finish
                _myThread = null;
            }

            //start a new thread
            _cancellationTokenSource = new CancellationTokenSource(); // Create a new token source
            _myThread = new Thread(() => ShowViewNonBlocking(_cancellationTokenSource.Token));
            _myThread.SetApartmentState(ApartmentState.STA);
            _myThread.Start();
        }

        /// <summary>
        /// supports the 'ShowDialogNonBlocking' method in opening the dialog view
        /// </summary>
        /// <param name="cancellationToken"></param>
        private void ShowViewNonBlocking(CancellationToken cancellationToken)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.View.Show();
            });

        }

        /// <summary>
        /// method closing the dialog in non-blocking mode
        /// </summary>
        private void CloseNonBlocking()
        {
            if (_myThread != null)
            {
                _cancellationTokenSource.Cancel(); // Cancel the token
                _myThread.Join(); // Wait for the thread to finish
                _myThread = null;
            }
        }

        #endregion


        #region private method(s) supporting events

        /// <summary>
        /// method for raising the 'Closed' event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosed(object sender, EventArgs e)
        {
            DialogClosed(this);
        }

        #endregion

    }

}