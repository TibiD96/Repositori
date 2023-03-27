using System;

namespace CollectionData
{
    class IntArray
    {
        private int[] input;
        private int position = 0;

        public IntArray()
        {
            this.input = new int[4];
        }

        public void Add(int element)
        {
            Resizing();
            input[position++] = element;
        }

        public int Count()
        {
            return position;
        }

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
            return input.Contains(element);
        }

        public int IndexOf(int element)
        {
            for(int i = 0; i < input.Length; i++)
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
            position++;
        }

        public void Clear()
        {
            position = 0;
        }

        public void Remove(int element)
        {
            int indexOfElementToBeRemoved = IndexOf(element);
            ShiftLeft(indexOfElementToBeRemoved);
            position--;
        }

        public void RemoveAt(int index)
        {
            ShiftLeft(index);
            position--;
        }

        private void ShiftLeft(int index)
        {
            for (int i = index; i <= position - 1; i++)
            {
                SetElement(i, input[i + 1]);
            }
        }

        private void ShiftRight(int index)
        {
            for (int i = position - 1; i >= index ; i--)
            {
                SetElement(input[i + 1], i);
            }
        }

        private void Resizing()
        {
            if (position == input.Length)
            {
                Array.Resize(ref input, input.Length * 2);
            }
        }
    }
}