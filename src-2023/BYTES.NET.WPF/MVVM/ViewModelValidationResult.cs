//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.WPF.MVVM
{
    /// <summary>
    /// the generic result type for ViewModel validation
    /// </summary>
    public class ViewModelValidationResult
    {
        #region public enum(s)

        public enum ResultType
        {
            Undefined = 0,
            Information = 1,
            Warning = 2,
            Error = 3
        }

        #endregion

        #region private variable(s)

        private ViewModelValidationResult.ResultType _type = ResultType.Undefined;
        private string _message = String.Empty;

        #endregion

        #region public properties

        public ViewModelValidationResult.ResultType Type
        {
            get => _type;
        }

        public string Message
        {
            get => _message;
        }

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public ViewModelValidationResult(string message, ResultType type = ResultType.Information)
        {
            _message = message;
            _type = type;
        }

        /// <summary>
        /// overloaded new instance method, supporting type declaration only
        /// </summary>
        /// <param name="type"></param>
        public ViewModelValidationResult(ResultType type)
        {
            _type = type;
        }

        #endregion
    }
}
