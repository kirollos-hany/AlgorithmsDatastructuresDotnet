namespace Kiro.Datastructures.List
{
    public class LinkedListNode<T>
    {
        public LinkedListNode<T>? Next { get; set; }
        
        public LinkedListNode<T>? Prev { get; set; }
        
        public T Item { get; set; }

        public LinkedListNode(T item)
        {
            Next = null;
            Prev = null;
            Item = item;
        }

        public override string ToString()
        {
            return Item.ToString();
        }

        public static implicit operator T(LinkedListNode<T> node) => node.Item;
    }
}