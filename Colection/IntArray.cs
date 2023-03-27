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

        private void ShiftRight(int index)
        {
            for (int i = index; i < input.Length - 1; i++)
            {
                SetElement(i, input[i + 1]);
            }
        }

        private void ShiftLeft(int index)
        {
            for (int i = input.Length - 1; i > index; i--)
            {
                SetElement(i, input[i - 1]);
            }
        }

        private void Resizing(ref int[] input)
        {
            Array.Resize(ref input, input.Length - 1);
        }

        public void Add(int element)
        {
            Array.Resize<int>(ref input, input.Length + 1);
            input[^1] = element;
        }

        public int Count()
        {
            return input.Length;
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
            Array.Resize<int>(ref input, input.Length + 1);
            ShiftLeft(index);
            SetElement(index, element);
        }

        public void Clear()
        {
            Array.Resize(ref input, 0);
        }

        public void Remove(int element)
        {
            int indexOfElementToBeRemoved = Array.IndexOf(input, element);
            ShiftRight(indexOfElementToBeRemoved);
            Resizing(ref input);
        }

        public void RemoveAt(int index)
        {
            ShiftRight(index);
            Resizing(ref input);
        }
    }
}