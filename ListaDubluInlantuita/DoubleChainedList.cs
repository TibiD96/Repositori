using System.Collections;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
