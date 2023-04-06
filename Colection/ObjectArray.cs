using System;
using System.Collections;

namespace CollectionData
{
    public class ObjectArray : IEnumerable
    {
        private object[] input;

        public ObjectArray()
        {
            this.input = new object[4];
        }

        public int Count { get; private set; }

        public object this[int index]
        {
            get => input[index];
            set => input[index] = value;
        }

        public void Add(object element)
        {
            Resizing();
            input[Count++] = element;
        }

        public IEnumerator GetEnumerator()
        {
            return new ObjectEnumerator(this);
        }

        public bool Contains(object element)
        {
            return IndexOf(element) > -1;
        }

        public int IndexOf(object element)
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

        public void Insert(int index, object element)
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

        public void Remove(object element)
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
