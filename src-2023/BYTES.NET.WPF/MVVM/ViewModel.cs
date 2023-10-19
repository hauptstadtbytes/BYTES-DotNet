using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
            PropertyChanged(this, new PropertyChangedEventArgs(property));

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

}
