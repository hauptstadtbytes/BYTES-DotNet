using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace BYTES.NET.WPF.MVVM
{
    /// <summary>
    /// ViewModel for a GUI Thread
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GUIThreadViewModel<T> : ViewModel where T : Window, new()
    {
        /// <summary>
        /// View for the ViewModel
        /// </summary>
        protected Window _myView;

        /// <summary>
        /// Thread for the ViewModel
        /// </summary>
        protected Thread _myThread;

        /// <summary>
        /// Cancellation token source for the ViewModel
        /// </summary>
        // went with cancellation token refer to https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Lambda expression for the View
        /// </summary>
        public Window View => _myView;

        /// <summary>
        /// Thread ID
        /// </summary>
        public int ThreadID => _myThread.ManagedThreadId;

        public event Action<GUIThreadViewModel<T>> Closed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="show"></param>
        public GUIThreadViewModel(bool show = true)
        {
            if (show)
            {
                Show();
            }
        }

        /// <summary>
        /// Shows the ViewModel
        /// </summary>
        public void Show()
        {
            if (_myThread != null)
            {
                this.Closed?.Invoke(this);
                _cancellationTokenSource.Cancel(); // Cancel the previous token
                _myThread.Join(); // Wait for the previous thread to finish
                _myThread = null;
            }
            _cancellationTokenSource = new CancellationTokenSource(); // Create a new token source
            _myThread = new Thread(() => ShowWindow(_cancellationTokenSource.Token));
            _myThread.SetApartmentState(ApartmentState.STA);
            _myThread.Start();
        }

        /// <summary>
        /// Closes the ViewModel
        /// </summary>
        public void Close()
        {
            if (_myThread != null)
            {
                _cancellationTokenSource.Cancel(); // Cancel the token
                _myThread.Join(); // Wait for the thread to finish
                _myThread = null;
            }
        }

        private void ShowWindow(CancellationToken cancellationToken)
        {
            _myView = new T();
            _myView.DataContext = this;
            _myView.Show();
            _myView.Closed += OnWindowClosed;
            Dispatcher.Run();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            this.Closed?.Invoke(this);
            _myView.Dispatcher.InvokeShutdown();
        }
    }
}
