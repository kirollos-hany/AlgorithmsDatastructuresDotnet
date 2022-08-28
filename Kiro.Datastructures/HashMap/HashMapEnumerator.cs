using System.Collections;
using System.Collections.Generic;

namespace Kiro.Datastructures.HashMap
{
    public class HashMapEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        private readonly HashMap<TKey, TValue> _map;

        private int _position;
        public HashMapEnumerator(HashMap<TKey, TValue> map)
        {
            _map = map;
            _position = -1;
        }
        public bool MoveNext()
        {
            _position++;
            return _position >= _map.Count;
        }

        public void Reset()
        {
            _position = -1;
        }

        public KeyValuePair<TKey, TValue> Current
        {
            get
            {
                var key = _map.KeysList[_position];
                var item = _map[key];
                return new KeyValuePair<TKey, TValue>(key, item);
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }
    }
}