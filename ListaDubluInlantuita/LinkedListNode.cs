using ChainedList;

namespace ChainedList
{
    public class LinkedListNode<T>
    {
        public LinkedListNode(T item)
        {
            Value = item;
        }

        public T Value { get; set; }

        public LinkedListNode<T> Right { get; set; }

        public LinkedListNode<T> Left { get; set; }

        public DoubleChainedList<T> List { get; internal set; }
    }
}
