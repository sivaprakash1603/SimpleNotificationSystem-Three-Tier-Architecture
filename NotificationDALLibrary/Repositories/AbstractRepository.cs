using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NotificationModelLibrary.Models;
using NotificationDALLibrary.Interfaces;

namespace NotificationDALLibrary.Repositories
{
    public abstract class AbstractRepository<K,T> : IRepository<K,T> where K : notnull where T : class
    {
        protected Dictionary<K, T>? _items;

        // Indexer for accessing items by key
        public T this[K index]
        {
            get { return _items![index]; }
            set { _items![index] = value; }
        }

        public abstract T? Create(T item);

        public T? Remove(K key)
        {
                if (_items==null||!_items.ContainsKey(key))
                {
                    return null;
                }
                T item = _items[key];
                _items.Remove(key);
                return item;
        }
        public T? Get(K key)
        {
            if (_items==null|| !_items.ContainsKey(key))
            {
                return null;
            }
            return _items[key];
        }
        public  List<T>? GetAll()
        {
            if(_items==null||_items.Count == 0)
            {
                return null;
            }
            return _items.Values.ToList();
        }

        public  T? Update(K key, T item)
        {
            if (_items==null||!_items.ContainsKey(key))
            {
                return null;
            }
            _items[key] = item;
            return item;
        }

            public bool Delete(K key)
            {
                if (_items==null||!_items.ContainsKey(key))
                {
                    return false;
                }
                return _items.Remove(key);
            }
    }
}