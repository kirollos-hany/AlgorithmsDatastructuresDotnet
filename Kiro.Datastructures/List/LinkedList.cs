using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Kiro.Datastructures.List
{
    public class LinkedList<T> : IList<T>, IReadOnlyList<T>
    {
        private LinkedListNode<T>? _head;

        private LinkedListNode<T>? _tail;

        private int _count;

        public IEnumerator<T> GetEnumerator()
        {
            return new LinkedListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (_head is null)
            {
                _head = new LinkedListNode<T>(item);
                _tail = _head;
                _count++;
                return;
            }

            var newNode = new LinkedListNode<T>(item);
            _tail!.Next = newNode;
            newNode.Prev = _tail;
            _tail = newNode;

            _count++;
        }

        public void Add(LinkedListNode<T> node)
        {
            if (_head is null)
            {
                _head = node;
                _tail = _head;
                _count++;
                return;
            }

            _tail!.Next = node;
            node.Prev = _tail;
            _tail = node;
            _count++;
        }

        public void Clear()
        {
            if (Count == 0)
            {
                return;
            }

            var tempPtr = _head;
            while (tempPtr != null)
            {
                var current = tempPtr;
                tempPtr = current.Next;
                current.Next = null;
                current.Prev = null;
            }

            _head = null;
            _tail = null;
            _count = 0;
        }

        public bool Contains(T item)
        {
            return Search(item) >= 0;
        }

        public bool Contains(LinkedListNode<T> node)
        {
            return Search(node) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CheckIndexBounds(arrayIndex);
            if (array.Length - arrayIndex < Count)
            {
                throw new IndexOutOfRangeException("Array offset size is less than size of the list");
            }

            var tempPtr = _head;
            for (var i = 0; i < Count; i++, arrayIndex++)
            {
                array[arrayIndex] = tempPtr.Item;
                tempPtr = tempPtr.Next;
            }
        }

        public bool Remove(T item)
        {
            var tempPtr = _head;
            while (tempPtr != null)
            {
                var tempItem = tempPtr.Item;
                if (tempItem.Equals(item))
                {
                    if (IsInBetween(tempPtr))
                    {
                        RemoveInBetween(tempPtr);
                    }
                    else if (IsHead(tempPtr))
                    {
                        RemoveHead(tempPtr);
                    }
                    else if (IsTail(tempPtr))
                    {
                        RemoveTail(tempPtr);
                    }

                    _count--;
                    return true;
                }

                tempPtr = tempPtr.Next;
            }

            return false;
        }

        private static bool IsTail(LinkedListNode<T> tempPtr)
        {
            return tempPtr.Next == null && tempPtr.Prev != null;
        }

        private static bool IsHead(LinkedListNode<T> tempPtr)
        {
            return tempPtr.Prev == null;
        }

        private static bool IsInBetween(LinkedListNode<T> tempPtr)
        {
            return tempPtr.Next != null && tempPtr.Prev != null;
        }

        private void RemoveTail(LinkedListNode<T> tempPtr)
        {
            _tail = tempPtr.Prev;
            var prevNode = tempPtr.Prev;
            prevNode!.Next = null;
        }

        private void RemoveHead(LinkedListNode<T> tempPtr)
        {
            if (tempPtr.Next != null)
            {
                _head = tempPtr.Next;
                _head.Prev = null;
                return;
            }

            _head = null;
        }

        private void RemoveInBetween(LinkedListNode<T> tempPtr)
        {
            var prevNode = tempPtr.Prev;
            prevNode!.Next = tempPtr.Next;
            var nextNode = tempPtr.Next;
            nextNode!.Prev = tempPtr.Prev;
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            return Search(item);
        }

        public int IndexOf(LinkedListNode<T> node)
        {
            return Search(node);
        }

        public void Insert(int index, T item)
        {
            CheckIndexBounds(index);

            var tempPtr = _head;
            var newNode = new LinkedListNode<T>(item);
            var i = 0;

            while (i < index)
            {
                tempPtr = tempPtr.Next;
                i++;
            }

            if (IsInBetween(tempPtr))
            {
                newNode.Next = tempPtr;
                newNode.Prev = tempPtr.Prev;
                var prevNode = tempPtr.Prev;
                prevNode!.Next = newNode;
                tempPtr.Prev = newNode;
            }
            else if (IsHead(tempPtr))
            {
                newNode.Next = tempPtr;
                _head = newNode;
            }
            else if (IsTail(tempPtr))
            {
                newNode.Next = tempPtr;
                var prevNode = tempPtr.Prev;
                prevNode.Next = newNode;
                newNode.Prev = prevNode;
                tempPtr.Prev = newNode;
            }
            
            _count++;
        }

        public void RemoveAt(int index)
        {
            CheckIndexBounds(index);

            var tempPtr = _head;
            var i = 0;
            while (i < index)
            {
                tempPtr = tempPtr.Next;
                i++;
            }

            if (IsInBetween(tempPtr))
            {
                RemoveInBetween(tempPtr);
            }
            else if (IsHead(tempPtr))
            {
                RemoveHead(tempPtr);
            }
            else if (IsTail(tempPtr))
            {
                RemoveTail(tempPtr);
            }

            _count--;
        }

        public T this[int index]
        {
            get => GetItem(index);
            set => SetItem(value, index);
        }

        public LinkedListNode<T> GetNode(int index)
        {
            CheckIndexBounds(index);

            var tempPtr = _head;
            var i = 0;
            while (i < index)
            {
                tempPtr = tempPtr.Next;
                i++;
            }

            return tempPtr;
        }

        public void SetNode(int index, LinkedListNode<T> node)
        {
            CheckIndexBounds(index);

            var tempPtr = _head;
            var i = 0;

            while (i < index)
            {
                tempPtr = tempPtr.Next;
                i++;
            }

            node.Next = tempPtr.Next;
            node.Prev = tempPtr.Prev;
            tempPtr = node;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder("[");
            foreach (var item in this)
            {
                stringBuilder.Append($"{item.ToString()}, ");
            }

            if (Count != 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 2, 2);
            }

            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        private T GetItem(int index)
        {
            CheckIndexBounds(index);

            var tempPtr = _head;
            var i = 0;
            while (i < index)
            {
                tempPtr = tempPtr.Next;
                i++;
            }

            return tempPtr.Item;
        }

        private void SetItem(T item, int index)
        {
            CheckIndexBounds(index);

            var i = 0;
            var tempPtr = _head;
            while (i < index)
            {
                tempPtr = tempPtr.Next;
            }

            tempPtr.Item = item;
        }

        private void CheckIndexBounds(int index)
        {
            if (index >= _count || index < 0) throw new IndexOutOfRangeException();
        }

        private int Search(T item)
        {
            var tempPtr = _head;
            var index = 0;
            while (tempPtr != null)
            {
                var tempItem = tempPtr.Item;
                if (tempItem.Equals(item))
                {
                    return index;
                }

                tempPtr = tempPtr.Next;
                index++;
            }

            return -1;
        }

        private int Search(LinkedListNode<T> node)
        {
            var tempPtr = _head;
            var index = 0;
            while (tempPtr != null)
            {
                if (tempPtr == node)
                {
                    return index;
                }

                tempPtr = tempPtr.Next;
                index++;
            }

            return -1;
        }
    }
}