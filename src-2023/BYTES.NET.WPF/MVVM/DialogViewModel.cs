using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;

namespace BYTES.NET.WPF.MVVM
{
    ///<summary>
    ///dialog viemodel base class
    ///</summary>
    public abstract class DialogViewModel : ViewModel
    {
        #region private Variables
        private bool _dialogResult = false;
        private EventWaitHandle _myBlock = null;
        #endregion

        #region public Properties
        /// <summary>
        /// dialog result
        /// </summary>
        public bool DialogResult
        {
            get { return _dialogResult; }
            set
            {
                _dialogResult = value;
                //OnPropertyChanged
            }
        }
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
        /// <returns></returns>
        public bool ShowDialog()
        {
            ShowView();

            _myBlock = new EventWaitHandle(false, EventResetMode.AutoReset);
            WaitForEvent (_myBlock, new TimeSpan(24, 0, 0));

            _myBlock.Dispose();

            return DialogResult;
        }
        /// <summary>
        /// method showing the vies
        /// </summary>
        protected abstract void ShowView();

        /// <summary>
        /// method closing the view
        /// </summary>
        protected abstract void CloseView();

        /// <summary>
        /// method closing the dialog, setting the result
        /// </summary>
        /// <param name="result"></param>
        protected void Close(bool result)
        {
            DialogResult = result;

            CloseView();

            if(_myBlock != null)
            {
                _myBlock.Set();
            }
        }
        #endregion

        #region private Methods
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
    }
}
