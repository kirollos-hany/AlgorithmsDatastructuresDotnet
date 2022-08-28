using System;
using System.Collections;
using System.Collections.Generic;

namespace Kiro.Datastructures.HashMap
{
    public class HashMap<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private int _count;

        private int _capacity;

        private double _loadFactor;

        private TValue[] _values;

        private readonly List<TKey> _keys;

        public HashMap()
        {
            _loadFactor = 0;
            _count = 0;
            _capacity = 100;
            _values = new TValue[_capacity];
            _keys = new List<TKey>(_capacity);
        }

        public HashMap(int capacity) : this()
        {
            _capacity = capacity;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new HashMapEnumerator<TKey, TValue>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            HandleLoad();
            var key = item.Key;
            var index = GetItemIndex(key);
            if (!_keys.Contains(key))
            {
                _keys.Add(key);
            }
            _values[index] = item.Value;
            _count++;
            _loadFactor = CalculateLoad();
        }

        private double CalculateLoad()
        {
            return (double)_count / _capacity;
        }

        private void HandleLoad()
        {
            if (!(_loadFactor >= 0.7)) return;
            _capacity *= 2;
            var temp = _values;
            _values = new TValue[_capacity];
            _keys.Capacity = _capacity;
            temp.CopyTo(_values, 0);
        }

        public void Clear()
        {
            _capacity = 100;
            _count = 0;
            _loadFactor = 0;
            _keys.Clear();
            _keys.Capacity = _capacity;
            _values = new TValue[_capacity];
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _keys.Contains(item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            CheckIndexBounds(arrayIndex);
            if (array.Length - arrayIndex < _count)
            {
                throw new IndexOutOfRangeException("Array offset size is less than size of the list");
            }

            for (; arrayIndex < _count; arrayIndex++)
            {
                var item = new KeyValuePair<TKey, TValue>(_keys[arrayIndex], _values[arrayIndex]);
                array[arrayIndex] = item;
            }
        }
        
        private void CheckIndexBounds(int index)
        {
            if (index >= _count || index < 0) throw new IndexOutOfRangeException();
        }

        private int GetItemIndex(TKey key)
        {
            return key!.GetHashCode() % _capacity;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var keyExists = _keys.Contains(item.Key);
            if (!keyExists) return false;
            var index = GetItemIndex(item.Key);
            _values[index] = default!;
            _count--;
            _keys.Remove(item.Key);
            _loadFactor = CalculateLoad();
            return true;
        }

        public int Count => _count;

        public bool IsReadOnly => false;
        
        public void Add(TKey key, TValue value)
        {
            var item = new KeyValuePair<TKey, TValue>(key, value);
            Add(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _keys.Contains(key);
        }

        public bool Remove(TKey key)
        {
            var keyExists = _keys.Contains(key);
            if (!keyExists)
            {
                return false;
            }

            var keyIndex = _keys.IndexOf(key);
            var keyItem = _keys[keyIndex];
            var itemIndex = GetItemIndex(keyItem);
            _values[itemIndex] = default!;
            _keys.RemoveAt(keyIndex);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!_keys.Contains(key))
            {
                value = default!;
                return false;
            }

            var itemIndex = GetItemIndex(key);
            value = _values[itemIndex];
            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                var itemIndex = GetItemIndex(key);
                return _values[itemIndex];
            }
            set => Add(key, value);
        }

        public ICollection<TKey> Keys => _keys;
        public ICollection<TValue> Values => _values;

        public IReadOnlyList<TKey> KeysList => _keys;
    }
}