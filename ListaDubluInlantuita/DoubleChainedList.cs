using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;


namespace ChainedList
{
    public class DoubleChainedList<T> : ICollection<T>
    {
        private readonly LinkedListNode<T> sentinel = new LinkedListNode<T>(default);
        public DoubleChainedList()
        {
            sentinel.Next = sentinel;
            sentinel.Previous = sentinel;
        }

        public int Count { get; private set; }

        public bool IsReadOnly { get; private set; }
        public void Add(T item)
        {

        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (LinkedListNode<T> current = sentinel.Next; current != sentinel; current = current.Next)
            {
                yield return current.Value;
            }
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
