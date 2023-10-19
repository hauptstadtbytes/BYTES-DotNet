using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BYTES.NET.WPF.MVVM
{
    /// <summary>
    /// the basic MVVM view model relay command type
    /// </summary>
    /// <remarks>based on the article found at 'http://www.cocktailsandcode.de/2012/04/mvvm-tutorial-part-3-viewmodelbase-und-relaycommand/'</remarks>
    public class ViewModelRelayCommand : ICommand
    {
        #region private variable(s)

        private bool? _canExecuteBool = null;
        private Predicate<object> _canExecuteDelegate = null;

        private Action _toBeExecutedWithoutParameter = null;
        private Action<object> _toBeExecutedWithParameter = null;

        private bool _executeAsync = false;

        #endregion

        #region public properties

        public bool RunAsync
        {
            get => _executeAsync;
            set => _executeAsync = value;
        }

        public bool IsEnabled
        {
            get => CanExecute(null);
            set
            {
                _canExecuteBool = value;
                OnCanExecuteChanged();
            }
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="toBeExecuted"></param>
        /// <param name="canExecute"></param>
        public ViewModelRelayCommand(Action toBeExecuted, bool canExecute = true)
        {
            _toBeExecutedWithoutParameter = toBeExecuted;
            _canExecuteBool = canExecute;
        }

        /// <summary>
        /// overloaded new instance method, supporting an 'object' type parameter for the delegated 'Action'
        /// </summary>
        /// <param name="toBeExecuted"></param>
        /// <param name="canExecute"></param>
        public ViewModelRelayCommand(Action<object> toBeExecuted, bool canExecute = true)
        {
            _toBeExecutedWithParameter = toBeExecuted;
            _canExecuteBool = canExecute;
        }

        /// <summary>
        /// overloaded new instance method, supporting a predicate parameter for calculating 'CanExecute'
        /// </summary>
        /// <param name="toBeExecuted"></param>
        /// <param name="canExecute"></param>
        public ViewModelRelayCommand(Action toBeExecuted, Predicate<object> canExecute)
        {
            _toBeExecutedWithoutParameter = toBeExecuted;
            _canExecuteDelegate = canExecute;
        }

        /// <summary>
        /// overloading new instance method, supporting an 'object' type parameter for the delegated 'Action' and a predicate parameter for calculating 'CanExecute'
        /// </summary>
        /// <param name="toBeExecuted"></param>
        /// <param name="canExecute"></param>
        public ViewModelRelayCommand(Action<object> toBeExecuted, Predicate<object> canExecute)
        {
            _toBeExecutedWithParameter = toBeExecuted;
            _canExecuteDelegate = canExecute;
        }

        #endregion

        #region public event(s) and method(s) implementing 'ICommand'

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (_canExecuteBool.HasValue)
            {

                if (_canExecuteBool != null)
                {
                    return (bool)_canExecuteBool;
                }

            }
            else
            {

                if (_canExecuteDelegate != null)
                {
                    return _canExecuteDelegate(parameter);
                }

            }

            //return the default value
            return true;
        }

        public void Execute(object parameter)
        {
            if (this.RunAsync)
            {
                ExecuteAsync();
            }
            else
            {
                if (_toBeExecutedWithoutParameter != null)
                {
                    _toBeExecutedWithoutParameter();
                }
                else if (_toBeExecutedWithParameter != null)
                {
                    _toBeExecutedWithParameter(parameter);
                }
            }
        }

        #endregion

        #region private method(s) supporting 'ICommand'

        /// <summary>
        /// calling the 'CanExecuteChanged' event on changing the 'CanExecute' property
        /// </summary>
        private void OnCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// running the delegated action async
        /// </summary>
        private async void ExecuteAsync()
        {

            if (_toBeExecutedWithoutParameter != null)
            {
                await Task.Run(_toBeExecutedWithoutParameter);
            }
            else if (_toBeExecutedWithParameter != null)
            {
                await Task.Run(() => _toBeExecutedWithParameter);
            }

        }

        #endregion
    }
}
