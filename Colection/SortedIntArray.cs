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
                sorted = false;
                for (int i = 0; i < Count - 1; i++)
                {
                    if (base.input[i])
                }
            }
        }
    }
}
