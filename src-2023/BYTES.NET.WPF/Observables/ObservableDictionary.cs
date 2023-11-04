//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.WPF.Observables
{
    /// <summary>
    /// a (basic) observable dictionary
    /// </summary>
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged
    {
        #region public event(s) implementing 'INotifyCollectionChanged'

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        #endregion

        #region public method(s), overriding the base class behavior

        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                base[key] = value;
                OnCollectionChanged();
            }
        }

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnCollectionChanged();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            base.Add(item.Key, item.Value);
            OnCollectionChanged();
        }

        public new void Clear()
        {
            base.Clear();
            OnCollectionChanged();
        }

        public new bool Remove(TKey key)
        {
            bool result = base.Remove(key);
            OnCollectionChanged();
            return result;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool result = base.Remove(item.Key);
            OnCollectionChanged();
            return result;
        }

        #endregion

        #region private method(s) supporting 'INotifyCollectionChanged'

        /// <summary>
        /// raises the 'CollectionChanged' event
        /// </summary>
        private void OnCollectionChanged()
        {
            if (this.CollectionChanged != null) //otherwise there might be a 'NullReferenceException'
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }      
        }

        #endregion
    }
}
