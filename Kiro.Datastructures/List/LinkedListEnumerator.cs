using System.Collections;
using System.Collections.Generic;

namespace Kiro.Datastructures.List
{
    internal class LinkedListEnumerator<T> : IEnumerator<T>
    {
        private int _position = -1;
        private readonly LinkedList<T> _list;

        public LinkedListEnumerator(LinkedList<T> list)
        {
            _list = list;
        }

        public bool MoveNext()
        {
            _position++;
            return _position < _list.Count;
        }

        public void Reset()
        {
            _position = -1;
        }

        public T Current => _list[_position];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }
    }
}