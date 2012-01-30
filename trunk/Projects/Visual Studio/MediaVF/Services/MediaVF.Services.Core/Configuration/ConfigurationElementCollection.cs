using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MediaVF.Services.Core.Configuration
{
    public abstract class ConfigurationElementCollection<T> : ConfigurationElementCollection, IEnumerable<T> where T : ConfigurationElement, new()
    {
        #region Properties

        /// <summary>
        /// Enumerable collection 
        /// </summary>
        List<T> _items;
        List<T> Items
        {
            get
            {
                if (_items == null)
                    _items = new List<T>();
                return _items;
            }
        }

        #endregion Properties

        #region Collection Overrides

        /// <summary>
        /// Add-remove-clear map
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        /// <summary>
        /// Create a DataTypeMappingElement
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        /// <summary>
        /// Adds element to enumerable collection and calls base add
        /// </summary>
        /// <param name="element"></param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            Items.Add((T)element);
            base.BaseAdd(element);
        }

        protected override void BaseAdd(int index, ConfigurationElement element)
        {
            Items.Insert(index, (T)element);

            base.BaseAdd(index, element);
        }

        #endregion Collection Overrides

        #region IEnumerable Implementation

        public T this[int i]
        {
            get { return (T)base.BaseGet(i); }
            set
            {
                if (BaseGet(i) != null)
                {
                    Items.RemoveAt(i);

                    BaseRemoveAt(i);
                }

                BaseAdd(i, value);
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();

        }

        #endregion IEnumerable Implementation
    }
}
