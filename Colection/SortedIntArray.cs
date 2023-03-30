using System;

namespace CollectionData
{
    class SortedIntArray : IntArray
    {
        public SortedIntArray()
            : base()
        {
        }

        public void Add(int element)
        {
            base.Add(element);
            Sorting();
        }

        public void Sorting()
        {
            bool sorted = true;
            while(sorted)
            {
                int length = Count;
                sorted = false;
                for (int i = 0; i < length - 1; i++)
                {
                    if (base.input[i] > input[i + 1])
                    {
                        int pivot = input[i];
                        input[i] = input[i + 1];
                        input[i + 1] = pivot;
                        sorted = true;
                    }
                }
                length--;
            }
        }
    }
}
