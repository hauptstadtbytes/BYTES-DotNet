//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.WPF.MVVM.Menu
{
    public class MenuItemViewModel : ViewModelBase
    {

        #region protected properties

        protected string _caption = string.Empty;

        protected ViewModelRelayCommand ?_command = null;
        protected object ?_commandParameters = null;

        protected ObservableCollection<MenuItemViewModel> _children = new ObservableCollection<MenuItemViewModel>();

        #endregion

        #region public properties

        public string Caption
        {
            get => _caption; set
            {
                _caption = value;
                OnPropertyChanged();
            }
        }

        public ViewModelRelayCommand? Command { get => _command; set
            {
                _command = value;
                OnPropertyChanged();
                OnPropertyChanged("IsEnabled");

            } }

        public object? CommandParameters
        {
            get => _commandParameters; set
            {
                _commandParameters = value;
                OnPropertyChanged();
                OnPropertyChanged("IsEnabled");

            }
        }

        public ObservableCollection<MenuItemViewModel> Children
        {
            get => _children; set
            {
                _children = value;
                OnPropertyChanged();
                OnPropertyChanged("IsEnabled");
            }
        }

        public bool IsEnabled { get
            {

                if(_command != null)
                {
                    return _command.IsEnabled;
                }

                if(_children.Count > 0)
                {
                    return true;
                }

                return false;

            } }


        #endregion


    }
}
