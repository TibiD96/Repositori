using System;

namespace CollectionData
{
    class IntArray
    {
        private int[] input;

        public IntArray()
        {
            this.input = new int[4];
        }

        public int Count { get; private set; }

        public int this[int index]
        {
            get => input[index];
            set => input[index] = value;
        }

        public void Add(int element)
        {
            Resizing();
            input[Count++] = element;
        }

        public bool Contains(int element)
        {
            return IndexOf(element) > -1;
        }

        public int IndexOf(int element)
        {
            for(int i = 0; i < Count; i++)
            {
                if (input[i] == element)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, int element)
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

        public void Remove(int element)
        {
            int indexOfElementToBeRemoved = IndexOf(element);
            RemoveAt(indexOfElementToBeRemoved);
        }

        public void RemoveAt(int index)
        {
            ShiftLeft(index);
            Count--;
        }

        private void ShiftLeft(int index)
        {
            for (int i = index; i <= Count - 1; i++)
            {
                input[i] = input[i + 1];
            }
        }

        private void ShiftRight(int index)
        {
            for (int i = Count - 1; i >= index ; i--)
            {
                input[i + 1] = input[i];
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