using System;
using System.Collections;

namespace CollectionData
{
    public class List<T> : IEnumerable<T>
    {
        private T[] input;

        public List()
        {
            this.input = new T[4];
        }

        public int Count { get; private set; }

        public virtual T this[int index]
        {
            get => input[index];
            set => input[index] = value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return input[i];
            }
        }

        public virtual void Add(T element)
        {
            Resizing();
            input[Count++] = element;
        }

        public bool Contains(T element)
        {
            return IndexOf(element) > -1;
        }

        public virtual int IndexOf(T element)
        {
            for (int i = 0; i < Count; i++)
            {
                if (input[i].Equals(element))
                {
                    return i;
                }
            }

            return -1;
        }

        public virtual void Insert(int index, T element)
        {
            Resizing();
            ShiftRight(index);
            input[index] = element;
            Count++;
        }

        public void Clear()
        {
            Count = 0;
        }

        public void Remove(T element)
        {
            int indexOfElementToBeRemoved = IndexOf(element);
            RemoveAt(indexOfElementToBeRemoved);
        }

        public void RemoveAt(int index)
        {
            ShiftLeft(index);
            Count--;
        }

        private void ShiftRight(int index)
        {
            for (int i = Count - 1; i >= index; i--)
            {
                input[i + 1] = input[i];
            }
        }

        private void ShiftLeft(int index)
        {
            for (int i = index; i <= Count - 1; i++)
            {
                input[i] = input[i + 1];
            }
        }

        private void Resizing()
        {
            if (Count == input.Length)
            {
                Array.Resize(ref input, input.Length * 2);
            }
        }
    }
}
