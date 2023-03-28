using System;

namespace CollectionData
{
    class IntArray
    {
        private int[] input;
        private int count = 0;

        public IntArray()
        {
            this.input = new int[4];
        }

        public void Add(int element)
        {
            Resizing();
            input[count++] = element;
        }

        public int Count { get; private set; }

        public int Element(int index)
        {
            return input[index];
        }

        public void SetElement(int index, int element)
        {
            input[index] = element;
        }

        public bool Contains(int element)
        {
            return IndexOf(element) > -1;
        }

        public int IndexOf(int element)
        {
            for(int i = 0; i < count; i++)
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
            SetElement(index, element);
            count++;
        }

        public void Clear()
        {
            count = 0;
        }

        public void Remove(int element)
        {
            int indexOfElementToBeRemoved = IndexOf(element);
            RemoveAt(indexOfElementToBeRemoved);
        }

        public void RemoveAt(int index)
        {
            ShiftLeft(index);
            count--;
        }

        private void ShiftLeft(int index)
        {
            for (int i = index; i <= count - 1; i++)
            {
                SetElement(i, input[i + 1]);
            }
        }

        private void ShiftRight(int index)
        {
            for (int i = count - 1; i >= index ; i--)
            {
                SetElement(input[i + 1], i);
            }
        }

        private void Resizing()
        {
            if (count == input.Length)
            {
                Array.Resize(ref input, input.Length * 2);
            }
        }
    }
}