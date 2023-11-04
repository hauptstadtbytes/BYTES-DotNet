//import .net namespace(s) required
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BYTES.NET.WPF.MVVM
{
    /// <summary>
    /// the (basic) MVVM ViewModel class
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region public event(s) implementing 'INotifyPropertyChanged'

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region public event(s) implementing 'INotifyDataErrorInfo'

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion

        #region private variable(s), supporting relay command(s)

        private Dictionary<string, ViewModelRelayCommand> _commands = new Dictionary<string, ViewModelRelayCommand>();

        #endregion

        #region private variable(s), supporting ViewModel validation

        private List<ViewModelValidationRule> _validationRules = new List<ViewModelValidationRule>();
        private Dictionary<string, List<ViewModelValidationResult>> _validationResults = new Dictionary<string, List<ViewModelValidationResult>>(StringComparer.OrdinalIgnoreCase);

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

        #region public propertie(s), supporting ViewModel validation

        public virtual List<ViewModelValidationRule> ValidationRules
        {
            get => _validationRules;
            set
            {
                _validationRules = value;
                OnPropertyChanged();
            }
        }

        public bool HasErrors { get => _validationResults.Count > 0; }

        #endregion

        #region public method(s), supporting ViewModel validation

        public IEnumerable? GetErrors(string propertyName)
        {
            if (_validationResults.ContainsKey(propertyName))
            {
                return _validationResults[propertyName].ToArray();
            }

            return null;
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

            //(re-)validate
            if (revalidate)
            {
                Validate(property);
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

        #region protected method(s) supporting 'INotifyDataErrorInfo'

        /// <summary>
        /// (re-)evaluates all properties given
        /// </summary>
        /// <param name="properties"></param>
        /// <remarks>if properties equal to 'null', all (public) properties will be (re-)validated</remarks>
        protected void Validate(string[] properties = null)
        {

            //clear the validation results
            _validationResults = new Dictionary<string, List<ViewModelValidationResult>>(StringComparer.OrdinalIgnoreCase);

            //get a list of all public properties
            if (properties == null)
            {
                List<string> props = new List<string>();

                foreach (PropertyInfo info in this.GetType().GetProperties(BindingFlags.Public))
                {
                    props.Add(info.Name);
                }

                properties = props.ToArray();
            }

            //get the validation result(s)
            foreach (string property in properties)
            {

                if (!_validationResults.ContainsKey(property))
                {
                    _validationResults.Add(property, new List<ViewModelValidationResult>());
                }

                foreach (ViewModelValidationRule rule in _validationRules)
                {

                    if (rule.Properties.Length == 0 || rule.Properties.Contains(property))
                    {

                        foreach (ViewModelValidationResult result in rule.Validate(property))
                        {
                            _validationResults[property].Add(result);
                        }

                    }

                }

            }

            //strip the dictionary
            List<string> keys = new List<string>();
            foreach (string key in _validationResults.Keys)
            {
                keys.Add(key);
            }

            foreach (string property in keys.ToArray())
            {
                if (_validationResults[property].Count < 1)
                {
                    _validationResults.Remove(property);
                }
            }

            //update the GUI
            OnPropertyChanged("HasErrors");
            OnErrorsChanged(properties);

        }

        /// <summary>
        /// overloaded method (re-)validating a single property given
        /// </summary>
        /// <param name="property"></param>
        protected void Validate(string property)
        {
            Validate(new string[] { property });
        }

        /// <summary>
        /// raises the 'ErrorsChanged' event
        /// </summary>
        /// <param name="properties"></param>
        protected void OnErrorsChanged(string[]? properties = null)
        {
            if (this.ErrorsChanged != null) //otherwise there might be a 'NullReferenceException'
            {
                if (properties == null)
                {
                    ErrorsChanged(this, new DataErrorsChangedEventArgs(null));
                }

                foreach (string property in properties)
                {
                    ErrorsChanged(this, new DataErrorsChangedEventArgs(property));
                }
            }
        }

        /// <summary>
        /// overloaded method, supporting to raise the 'ErrorsChanged' event for a dedicated property
        /// </summary>
        /// <param name="property"></param>
        protected void OnErrorsChanged(string property)
        {
            OnErrorsChanged(new string[] { property });
        }

        /// <summary>
        /// return a dictionary of all errors by property name
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Dictionary<string, List<ViewModelValidationResult>> GetErrors(string[] properties = null)
        {
            Dictionary<string, List<ViewModelValidationResult>> output = new Dictionary<string, List<ViewModelValidationResult>>();

            //add validation errors from current instance
            if (properties == null)
            {

                output = new Dictionary<string, List<ViewModelValidationResult>>(_validationResults, StringComparer.OrdinalIgnoreCase);

            }
            else
            {

                foreach (string property in properties)
                {
                    if (_validationResults.ContainsKey(property))
                    {
                        output.Add(property, _validationResults[property]);
                    }
                }

            }

            return output;
        }

        #endregion
    }

    /// <summary>
    /// the MVVM view model class, supporing view type definition
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
