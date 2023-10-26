//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BYTES.NET.WPF.MVVM
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        #region public event(s) for implementing 'INotifyPropertyChanged'

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region private variable(s), supporting relay command(s)

        private Dictionary<string, ViewModelRelayCommand> _commands = new Dictionary<string, ViewModelRelayCommand>();

        #endregion

        #region public propertie(s), supporting relay command(s)

        public Dictionary<string, ViewModelRelayCommand> Commands
        {
            get => _commands;
            set
            {
                _commands = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region protected method(s) supporting 'INotifyPropertyChanged'

        /// <summary>
        /// nofifies on property change(s)
        /// </summary>
        /// <param name="revalidate"></param>
        /// <param name="property"></param>
        /// <remarks>see also 'http://jobijoy.blogspot.de/2009/07/easy-way-to-update-all-ui-property.html' for details</remarks>
        protected void OnPropertyChanged(bool revalidate, [CallerMemberName] string property = null)
        {

            //raise the 'PropertyChanged' event
            if(this.PropertyChanged != null) //otherwise there might be a 'NullReferenceException' when using 'this.<property>' i.e. in constructor
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }

        }

        /// <summary>
        /// overloading method for notifying on property change(s)
        /// </summary>
        /// <param name="property"></param>
        /// <param name="revalidate"></param>
        protected void OnPropertyChanged([CallerMemberName] string property = null, bool revalidate = false)
        {
            OnPropertyChanged(revalidate, property);
        }

        /// <summary>
        /// overloaded method, supporting an array of property name(s)
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="revalidate"></param>
        protected void OnPropertyChanged(string[] properties, bool revalidate = false)
        {
            foreach (string property in properties)
            {
                OnPropertyChanged(revalidate, property);
            }
        }

        #endregion
    }

    /// <summary>
    /// generic MVVM view model class, supporing a view type definition
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    public abstract class ViewModel<TView> : ViewModel where TView : Control
    {
        #region private variable(s)

        private TView _view = default(TView);

        #endregion

        #region public properties

        public TView View
        {
            get
            {
                if (EqualityComparer<TView>.Default.Equals(_view, default(TView)))
                {
                    _view = (TView)Activator.CreateInstance(typeof(TView));

                    _view.DataContext = this;
                }

                return _view;
            }
            set
            {
                _view = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }

}
