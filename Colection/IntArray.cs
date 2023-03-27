using System;

namespace CollectionData
{
    class IntArray
    {
        private int[] input;

        public IntArray()
        {
            this.input = new int[0];
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
            for(int i = input.Length - 1; i > index; i--)
            {
                SetElement(i, input[i - 1]);
            }

            SetElement(index, element);
        }

        public void Clear()
        {
            Array.Resize(ref input, 0);
        }

        private void Shift(int index)
        {
            for (int i = index; i < input.Length - 1; i++)
            {
                SetElement(i, input[i + 1]);
            }
        }

        private void ShortenTheArray(ref int[] input)
        {
            Array.Resize(ref input, input.Length - 1);
        }

        public void Remove(int element)
        {
            int indexOfElementToBeRemoved = Array.IndexOf(input, element);
            Shift(indexOfElementToBeRemoved);
            ShortenTheArray(ref input);
        }

        public void RemoveAt(int index)
        {
            Shift(index);
            ShortenTheArray(ref input);
        }
    }
}