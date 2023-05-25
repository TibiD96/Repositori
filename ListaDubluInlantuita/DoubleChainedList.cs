using System.Collections;


namespace ChainedList
{
    public class DoubleChainedList<T> : ICollection<T>
    {
        private readonly LinkedListNode<T> sentinel = new LinkedListNode<T>(default);
        public DoubleChainedList()
        {
            sentinel.Right = sentinel;
            sentinel.Left = sentinel;
        }

        public DoubleChainedList(T[] data)
        {
            sentinel.Right = sentinel;
            sentinel.Left = sentinel;
            foreach (var element in data)
            {
                Add(element);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (LinkedListNode<T> current = sentinel.Right; current != sentinel; current = current.Right)
            {
                yield return current.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; private set; }

        public bool IsReadOnly { get; private set; }

        public LinkedListNode<T> GetFirst
        {
            get
            {
                return sentinel.Right;
            }
        }

        public LinkedListNode<T> GetLast
        {
            get
            {
                return sentinel.Left;
            }
        }
        public void Add(T item)
        {
            AddLast(item);
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

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> addThis)
        {
            throw new NotImplementedException();
        }

        public void AddAfter(LinkedListNode<T> node, T item)
        {
            throw new NotImplementedException();
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            throw new NotImplementedException();
        }

        public void AddFirst(T item)
        {
            throw new NotImplementedException();
        }

        public void AddLast(T item)
        {
            LinkedListNode<T> addThis = new LinkedListNode<T>(item);
            AddLast(addThis);
        }

        public void AddLast(LinkedListNode<T> addThis)
        {
            AddBefore(sentinel, addThis);

        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> addThis)
        {
            node.Left.Right = addThis;
            addThis.Left = node.Left;
            node.Left = addThis;
            addThis.Right = node;
            addThis.List = this;
            Count++;
        }

        public void AddBefore(LinkedListNode<T> node, T item)
        {
            throw new NotImplementedException();
        }

        public void Find(T item)
        {
            throw new NotImplementedException();
        }

        public void FindLast(T item)
        {
            throw new NotImplementedException();
        }

        public void Remove(LinkedListNode<T> node)
        {
            throw new NotImplementedException();
        }

        public void RemoveFirst()
        {
            throw new NotImplementedException();
        }

        public void RemoveLast()
        {
            throw new NotImplementedException();
        }
    }
}
