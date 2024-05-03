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
using BYTES.NET.WPF.MVVM;

namespace BYTES.NET.WPF.MVVM
{
    ///<summary>
    ///dialog viemodel base class
    ///</summary>
    public abstract class DialogViewModel : ViewModel
    {
        #region private Variables for blocking dialog
        private bool _dialogResult = false;
        private EventWaitHandle _myBlock = null;
        #endregion

        #region private Variables for non-blocking dialog
        /// <summary>
        /// Cancellation token source for the ViewModel
        /// </summary>
        // went with cancellation token refer to https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        #endregion

        #region public Properties for blocking dialog
        /// <summary>
        /// dialog result
        /// </summary>
        public bool DialogResult
        {
            get { return _dialogResult; }
            set
            {
                _dialogResult = value;
                OnPropertyChanged();
                if (_dialogResult)
                {
                    //windowclosed event werfen
                }
            }
        }
        #endregion

        #region public Properties for non-blocking dialog
        /// <summary>
        /// Thread ID
        /// </summary>
        public int ThreadID => _myThread.ManagedThreadId;

        /// <summary>
        /// event for closing the dialog
        /// </summary>
        public event Action<DialogViewModel> Closed;
        #endregion

        #region protected Properties for non-blocking dialog
        /// <summary>
        /// View for the ViewModel
        /// </summary>
        protected Window _myView;

        /// <summary>
        /// Lambda expression for the View
        /// </summary>
        public Window View => _myView;

        /// <summary>
        /// Thread for the ViewModel
        /// </summary>
        protected Thread _myThread;
        #endregion

        #region instance Method
        /// <summary>
        /// default constructor
        /// </summary>
        public DialogViewModel()
        {
            
        }
        #endregion

        #region public Methods
        /// <summary>
        /// method showing up the overlay as dialog
        /// </summary>
        /// <param name="isBlocking"></param>
        public void ShowDialog(bool isBlocking)
        {
            if (isBlocking)
            {
                ShowDialogBlocking();
            }
            else
            {
                ShowDialogNonBlocking();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CloseDialog(bool isBlocking)
        {
            DialogResult = true;
            if (!isBlocking)
            {
                CloseNonBlocking();
            }
        }
        /// <summary>
        /// instatiates the view
        /// </summary>
        /// <param name="MyView"></param>
        public void instatiateView(Window MyView)
        {
            _myView = MyView;
            _myView.DataContext = this;
        }
        #endregion

        #region public Methods for blocking dialog
        /// <summary>
        /// method showing up the overlay as dialog
        /// </summary>
        /// <returns></returns>
        public bool ShowDialogBlocking()
        { 
            _myBlock = new EventWaitHandle(false, EventResetMode.AutoReset);
            _myView.ShowDialog();
            WaitForEvent (_myBlock, new TimeSpan(24, 0, 0));          
            _myBlock.Dispose();
            return DialogResult;
        }


        /// <summary>
        /// method closing the dialog, setting the result
        /// </summary>
        /// <param name="result"></param>
        protected void CloseBlocking(bool result)
        {
            DialogResult = result;

            if(_myBlock != null)
            {
                _myBlock.Set();
            }
        }
        #endregion

        #region public Methods for non-blocking dialog
        /// <summary>
        /// Shows the ViewModel
        /// </summary>
        public void ShowDialogNonBlocking()
        {
            //instatiateView(MyView);
            if (_myThread != null)
            {
                this.Closed?.Invoke(this);
                _cancellationTokenSource.Cancel(); // Cancel the previous token
                _myThread.Join(); // Wait for the previous thread to finish
                _myThread = null;
            }
            _cancellationTokenSource = new CancellationTokenSource(); // Create a new token source
            _myThread = new Thread(() => ShowViewNonBlocking(_cancellationTokenSource.Token));
            _myThread.SetApartmentState(ApartmentState.STA);
            _myThread.Start();
        }

        /// <summary>
        /// Closes the ViewModel
        /// </summary>
        public void CloseNonBlocking()
        {
            if (_myThread != null)
            {
                _cancellationTokenSource.Cancel(); // Cancel the token
                _myThread.Join(); // Wait for the thread to finish
                _myThread = null;
            }
        }

        #endregion

        #region private Methods for blocking dialog
        /// <summary>
        /// method allowing to wait for an event (non-blocking the application thread)
        /// </summary>
        /// <param name="waitHandle"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private bool WaitForEvent(EventWaitHandle waitHandle, TimeSpan? timeout = null)
        {
            bool didWait = false;
            DispatcherFrame frame = new DispatcherFrame();
            
            // ParameterizedThreadStart delegation
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart((obj) =>
            {
                didWait = waitHandle.WaitOne(timeout ?? TimeSpan.FromHours(24));
                frame.Continue = false;
            });

            Thread thread = new Thread(threadStart);
            thread.Start();
            Dispatcher.PushFrame(frame);
            return didWait;
        }
        #endregion

        #region private Methods for non-blocking dialog
        private void ShowViewNonBlocking(CancellationToken cancellationToken)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _myView = View;
                _myView.DataContext = this;
                _myView.Show();
                _myView.Closed += OnWindowClosed;
            });

        }
        private void OnWindowClosed(object sender, EventArgs e)
        {
            this.Closed?.Invoke(this);
        }
    }
    #endregion
}