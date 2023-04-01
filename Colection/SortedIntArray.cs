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

        public void Insert(int index, int element)
        {
            base.Insert(index, element);
            Sorting();
        }

        public void Remove(int element)
        {
            base.Remove(element);
        }

        private void Sorting()
        {
            bool sorted = true;
            while(sorted)
            {
                int length = Count;
                sorted = false;
                for (int i = 0; i < length - 1; i++)
                {
                    if (base[i] > base[i + 1])
                    {
                        int pivot = base[i];
                        base[i] = base[i + 1];
                        base[i + 1] = pivot;
                        sorted = true;
                    }
                }
                length--;
            }
        }
    }
}
