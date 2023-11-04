//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.WPF.MVVM
{
    /// <summary>
    /// the generic rule type for ViewModel validation
    /// </summary>
    public class ViewModelValidationRule
    {
        #region private variable(s)

        private Func<ViewModelValidationResult[]> _toBeExecutedResult = null;
        private Func<bool?> _toBeExecutedBool = null;

        private string[] _properties = null;
        private string _errorMessage = null;

        #endregion

        #region public properties

        public string[] Properties
        {
            get
            {
                if (_properties == null)
                {
                    return new string[] { };
                }

                return _properties;
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => _errorMessage = value;
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="toBeExecuted"></param>
        /// <param name="properties"></param>
        public ViewModelValidationRule(Func<ViewModelValidationResult[]> toBeExecuted, string[] properties = null)
        {
            _toBeExecutedResult = toBeExecuted;
            _properties = properties;
        }

        /// <summary>
        /// overloaded new instance method, supporting a single property name
        /// </summary>
        /// <param name="toBeExecuted"></param>
        /// <param name="property"></param>
        public ViewModelValidationRule(Func<ViewModelValidationResult[]> toBeExecuted, string property)
        {
            _toBeExecutedResult = toBeExecuted;
            _properties = new string[] { property };
        }

        /// <summary>
        /// overloaded new instance method, supporting a 'bool' result type delegated function
        /// </summary>
        /// <param name="toBeExecuted"></param>
        /// <param name="properties"></param>
        public ViewModelValidationRule(Func<bool?> toBeExecuted, string[] properties = null)
        {
            _toBeExecutedBool = toBeExecuted;
            _properties = properties;
        }

        /// <summary>
        /// overloaded new instance method, supporting a 'bool' result type delegated function for a single property
        /// </summary>
        /// <param name="toBeExecuted"></param>
        /// <param name="property"></param>
        public ViewModelValidationRule(Func<bool?> toBeExecuted, string property)
        {
            _toBeExecutedBool = toBeExecuted;
            _properties = new string[] { property };
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// return the validation result
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public ViewModelValidationResult[] Validate(string property = null)
        {
            if (_toBeExecutedResult != null)
            {
                return _toBeExecutedResult();
            }

            bool? result = _toBeExecutedBool();

            if (!result.HasValue)
            {
                return new ViewModelValidationResult[] { new ViewModelValidationResult(ViewModelValidationResult.ResultType.Undefined) };
            }

            if (result == true)
            {

                return new ViewModelValidationResult[] { new ViewModelValidationResult(ViewModelValidationResult.ResultType.Undefined) };

            }

            string message = "Validation failed";

            if (_properties != null)
            {
                if (_properties.Length == 1)
                {
                    message = "Validating '" + _properties[0] + "' failed";
                }
            }

            if (property != null)
            {
                message = "Validating '" + property + "' failed";
            }

            if (_errorMessage != null)
            {
                message = _errorMessage;
            }

            return new ViewModelValidationResult[] { new ViewModelValidationResult(message, ViewModelValidationResult.ResultType.Error) };

        }

        #endregion
    }
}
