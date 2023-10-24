//import .net (default) namespace(s)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET.WPF' framework
using BYTES.NET.WPF.MVVM;

//import internal namespace(s) required
using BYTES.NET.WPF.App.Views;

namespace BYTES.NET.WPF.App.ViewModels
{
    public class AnimalVM : ViewModel<AnimalView>
    {
        #region private variable(s)

        private string _name;
        private string _type;

        #endregion

        #region public properties

        public string AnimalName
        {
            get => _name; set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string AnimalType
        {
            get => _type; set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region public new instance method(s)

        public AnimalVM(string name, string type)
        {
            _name = name;
            _type = type;
        }

        #endregion

        }
}
