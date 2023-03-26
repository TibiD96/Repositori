using System;
using System.Reflection;

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
            if (input.Contains(element))
            {
                return true;
            }

            return false;
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
            Array.Resize<int>(ref input, 0);
        }

        public void Remove(int element)
        {
            int indexOfElementToBeRemoved = Array.IndexOf(input, element);
            for (int i = indexOfElementToBeRemoved; i < input.Length - 1; i++)
            {
                SetElement(i, input[i + 1]);
            }
            Array.Resize<int>(ref input, input.Length - 1);
        }

        public void RemoveAt(int index)
        {
            // șterge elementul de pe poziția dată
        }

    }

}