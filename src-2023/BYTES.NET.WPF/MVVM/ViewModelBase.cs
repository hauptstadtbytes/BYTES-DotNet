//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BYTES.NET.WPF.MVVM
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region public event(s) implementing 'INotifyPropertyChanged'

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region protected method(s) supporting 'INotifyPropertyChanged'

        /// <summary>
        /// nofifies on property change(s)
        /// </summary>
        /// <param name="property"></param>
        /// <remarks>see also 'http://jobijoy.blogspot.de/2009/07/easy-way-to-update-all-ui-property.html' for details</remarks>
        protected void OnPropertyChanged([CallerMemberName] string property = null)
        {

            //raise the 'PropertyChanged' event
            if (this.PropertyChanged != null) //otherwise there might be a 'NullReferenceException' when using 'this.<property>' i.e. in constructor
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }

        }

        /// <summary>
        /// overloaded method, supporting an array of property name(s)
        /// </summary>
        /// <param name="properties"></param>
        protected void OnPropertyChanged(string[] properties)
        {
            foreach (string property in properties)
            {
                OnPropertyChanged(property);
            }
        }

        /// <summary>
        /// notifies on all properties changed
        /// </summary>
        protected void OnAllPropertiesChanged()
        {

            //get the (public and static) properties
            foreach(PropertyInfo propInfo in this.GetType().GetProperties()){

                OnPropertyChanged(propInfo.Name);

            }

        }

        #endregion

    }
}
