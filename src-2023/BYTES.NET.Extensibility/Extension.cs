//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Extensibility
{
    /// <summary>
    /// the (basic) extension class
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    public class Extension<TInterface> : Lazy<TInterface>
    {
        #region private variable(s)

        private Type _type = null;
        private TInterface _instance = default;

        #endregion

        #region public Properties

        /// <summary>
        /// returns the value type without initializing
        /// </summary>
        public Type ValueType { get => _type; }

        /// <summary>
        /// check if the value was initialized or not
        /// </summary>
        public new bool IsValueCreated { get => base.IsValueCreated || _instance != null; }

        #endregion

        #region public new instance mehod(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valueFactory"></param>
        public Extension(Type type, Func<TInterface> valueFactory) : base(valueFactory)
        {
            _type = type;
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// returns a (newly created) value instance 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="independent"></param>
        /// <returns></returns>
        public new TInterface Value(object[] parameters, bool independent = true)
        {
            TInterface instance = (TInterface)Activator.CreateInstance(_type, parameters);

            if (independent)
            {
                return instance;
            }

            _instance = instance;
            return _instance;
        }

        /// <summary>
        /// returns the value
        /// </summary>
        /// <returns></returns>
        public new TInterface Value()
        {
            if (_instance != null)
            {
                return _instance;
            }

            return base.Value;
        }

        #endregion
    }

    /// <summary>
    /// the extension class, supporting metadata handling
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TMetadata"></typeparam>
    public class Extension<TInterface, TMetadata> : Lazy<TInterface, TMetadata>
    {
        #region private variable(s)

        private Type _type = null;
        private TInterface _instance = default;

        #endregion

        #region public Properties

        /// <summary>
        /// returns the value type without initializing
        /// </summary>
        public Type ValueType
        {
            get => _type;
        }

        /// <summary>
        /// check if the value was initialized or not
        /// </summary>
        public new bool IsValueCreated
        {
            get => base.IsValueCreated || _instance != null;
        }

        #endregion

        #region public new instance mehod(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valueFactory"></param>
        public Extension(Type type, Func<TInterface> valueFactory, TMetadata metadata) : base(valueFactory, metadata)
        {
            _type = type;
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// returns a (newly created) value instance 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="independent"></param>
        /// <returns></returns>
        public new TInterface Value(object[] parameters, bool independent = true)
        {
            TInterface instance = (TInterface)Activator.CreateInstance(_type, parameters);

            if (independent)
            {
                return instance;
            }

            _instance = instance;
            return _instance;
        }

        //returns the value
        public new TInterface Value()
        {
            if (_instance != null)
            {
                return _instance;
            }

            return base.Value;
        }

        #endregion
    }
}
