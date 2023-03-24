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
            if (input.Contains(element))
            {
                return true;
            }

            return false;
        }

        /*public int IndexOf(int element)
        {
            // întoarce indexul elementului sau -1 dacă elementul nu se
            // regăsește în șir
        } */

        public void Insert(int index, int element)
        {
            // adaugă un nou element pe poziția dată
        }

        public void Clear()
        {
            // șterge toate elementele din șir
        }

        public void Remove(int element)
        {
            // șterge prima apariție a elementului din șir	
        }

        public void RemoveAt(int index)
        {
            // șterge elementul de pe poziția dată
        }

    }

}