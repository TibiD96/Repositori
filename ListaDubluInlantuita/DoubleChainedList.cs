using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit.Sdk;


namespace ChainedList
{
    public class DoubleChainedList<T> : ICollection<T>
    {
        private readonly LinkedListNode<T> sentinel = new LinkedListNode<T>(default);
        public DoubleChainedList()
        {
            sentinel.Right = sentinel;
            sentinel.Left = sentinel;
            sentinel.List = this;
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

        public LinkedListNode<T> First
        {
            get
            {
                return CheckIfEmptyList(sentinel.Right);
            }
        }

        public LinkedListNode<T> Last
        {
            get
            {
                return CheckIfEmptyList(sentinel.Left);
            }
        }
        public void Add(T item)
        {
            AddLast(item);
        }

        public void Clear()
        {
            sentinel.Right = sentinel;
            sentinel.Left = sentinel;
            Count = 0;
        }

        public bool Contains(T item)
        {
            return Find(item) != null;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            LinkedListNode<T> removeThis = Find(item);
            return removeThis != null ? Remove(removeThis) : false;
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> addThis)
        {
            InexistentNode(node);
            NodeNUllException(addThis);
            AddBefore(node.Right, addThis);
        }

        public void AddAfter(LinkedListNode<T> node, T item)
        {
            ItemNUllException(item);
            InexistentNode(node);
            LinkedListNode<T> addThis = new LinkedListNode<T>(item);
            AddAfter(node, addThis);
        }

        public void AddFirst(LinkedListNode<T> addThis)
        {
            NodeNUllException(addThis);
            AddAfter(sentinel, addThis);
        }

        public void AddFirst(T item)
        {
            ItemNUllException(item);
            LinkedListNode<T> addThis = new LinkedListNode<T>(item);
            AddFirst(addThis);
        }

        public void AddLast(T item)
        {
            ItemNUllException(item);
            LinkedListNode<T> addThis = new LinkedListNode<T>(item);
            AddLast(addThis);
        }

        public void AddLast(LinkedListNode<T> addThis)
        {
            NodeNUllException(addThis);
            AddBefore(sentinel, addThis);

        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> addThis)
        {
            InexistentNode(node);
            NodeNUllException(addThis);
            node.Left.Right = addThis;
            addThis.Left = node.Left;
            node.Left = addThis;
            addThis.Right = node;
            addThis.List = this;
            Count++;
        }

        public void AddBefore(LinkedListNode<T> node, T item)
        {
            ItemNUllException(item);
            InexistentNode(node);
            LinkedListNode<T> addThis = new LinkedListNode<T>(item);
            AddBefore(node, addThis);
        }

        public LinkedListNode<T> Find(T item)
        {
            for(LinkedListNode<T>input = sentinel.Right; input != sentinel; input = input.Right)
            {
                if (input.Value.Equals(item))
                {
                    return input;
                }
            }

            return null;
        }

        public LinkedListNode<T> FindLast(T item)
        {
            for (LinkedListNode<T> input = sentinel.Left; input != sentinel; input = input.Left)
            {
                if (input.Value.Equals(item))
                {
                    return input;
                }
            }

            return null;
        }

        public bool Remove(LinkedListNode<T> node)
        {
            NodeNUllException(node);
            node.Left.Right = node.Right;
            node.Right.Left = node.Left;
            Count--;
            return true;
        }

        public bool RemoveFirst()
        {
            LinkedListNode<T> input = sentinel.Right;
            return CheckIfEmptyList(input) != null ? Remove(input) : throw new ArgumentNullException("Item can't bee null");
        }

        public bool RemoveLast()
        {
            LinkedListNode<T> input = sentinel.Left;
            return CheckIfEmptyList(input) != null ? Remove(input) : throw new ArgumentNullException("Item can't bee null");
        }

        public LinkedListNode<T> CheckIfEmptyList(LinkedListNode<T> node)
        {
            if (Count ==  0)
            {
                return null;
            }

            return node;
        }

        public void NodeNUllException(LinkedListNode<T> node)

        {
            if (node == null)
            {
                throw new ArgumentNullException("Node can't bee null");
            }

            return;
        }

        public void ItemNUllException(T item)

        {
            if (item == null)
            {
                throw new ArgumentNullException("Item can't bee null");
            }

            return;
        }

        public void InexistentNode(LinkedListNode<T> node)
        {
            if(node.List != this)
            {
                throw new InvalidOperationException("Node is not part of the list");
            }

            return;
        }
    }
}
