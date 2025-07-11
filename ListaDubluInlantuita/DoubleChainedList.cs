﻿using System.Collections;
using System.Collections.Generic;

namespace ChainedList
{
    public class DoubleChainedListCollection<T> : ICollection<T>
    {
        private readonly LinkedListNode<T> sentinel = new LinkedListNode<T>(default);

        public DoubleChainedListCollection()
        {
            sentinel.Right = sentinel;
            sentinel.Left = sentinel;
            sentinel.List = this;
        }

        public int Count { get; private set; }

        public bool IsReadOnly { get; set; }

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

        public void Add(T item)
        {
            AddLast(item);
        }

        public void Clear()
        {
            NotSupportedException();
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
            ArrayNullException(array);
            int index = 0;
            for (LinkedListNode<T> input = sentinel.Right; input != sentinel; input = input.Right)
            {
                array[index] = input.Value;
                index++;
            }
        }

        public bool Remove(T item)
        {
            NotSupportedException();
            LinkedListNode<T> removeThis = Find(item);
            return removeThis != null && Remove(removeThis);
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> addThis)
        {
            NodeNullException(node);
            AddBefore(node.Right, addThis);
        }

        public void AddAfter(LinkedListNode<T> node, T item)
        {
            AddAfter(node, new LinkedListNode<T>(item));
        }

        public void AddFirst(LinkedListNode<T> addThis)
        {
            AddAfter(sentinel, addThis);
        }

        public void AddFirst(T item)
        {
            AddFirst(new LinkedListNode<T>(item));
        }

        public void AddLast(T item)
        {
            AddLast(new LinkedListNode<T>(item));
        }

        public void AddLast(LinkedListNode<T> addThis)
        {
            AddBefore(sentinel, addThis);
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> addThis)
        {
            NodeNullException(addThis);
            NodeIsPartOfAnotherList(addThis);
            NodeNullException(node);
            InexistentNode(node);
            node.Left.Right = addThis;
            addThis.Left = node.Left;
            node.Left = addThis;
            addThis.Right = node;
            addThis.List = this;
            Count++;
        }

        public void AddBefore(LinkedListNode<T> node, T item)
        {
            AddBefore(node, new LinkedListNode<T>(item));
        }

        public LinkedListNode<T> Find(T item)
        {
            for (LinkedListNode<T> input = sentinel.Right; input != sentinel; input = input.Right)
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
            NodeNullException(node);
            node.Left.Right = node.Right;
            node.Right.Left = node.Left;
            Count--;
            node.List = null;
            return true;
        }

        public bool RemoveFirst()
        {
            LinkedListNode<T> input = sentinel.Right;
            return CheckIfEmptyList(input) != null && Remove(input);
        }

        public bool RemoveLast()
        {
            LinkedListNode<T> input = sentinel.Left;
            return CheckIfEmptyList(input) != null && Remove(input);
        }

        public LinkedListNode<T> CheckIfEmptyList(LinkedListNode<T> node)
        {
            if (Count == 0)
            {
                return null;
            }

            return node;
        }

        public void NodeNullException(LinkedListNode<T> node)
        {
            if (node != null)
            {
                return;
            }

            throw new ArgumentNullException("node");
        }

        public void ArrayNullException(T[] array)
        {
            if (array != null)
            {
                return;
            }

            throw new ArgumentNullException("array");
        }

        public void NotSupportedException()
        {
            if (!IsReadOnly)
            {
                return;
            }

            throw new NotSupportedException("Is Read Only");
        }

        public void InexistentNode(LinkedListNode<T> node)
        {
            if (node.List == this)
            {
                return;
            }

            throw new InvalidOperationException("Node is not part of the list");
        }

        public void NodeIsPartOfAnotherList(LinkedListNode<T> node)
        {
            if (node.List == null)
            {
                return;
            }

            throw new InvalidOperationException("Node is  part of different list");
        }
    }
}
