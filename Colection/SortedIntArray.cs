using System;

namespace CollectionData
{
    public class SortedIntArray : IntArray
    {
        public SortedIntArray()
            : base()
        {
        }

        public override void Add(int element)
        {
            base.Add(element);
            Sorting();
        }
        public override int this[int index]
        {
            set
             {
                if (CheckTheElementToNotBreakTheSorting(index, value))
                {
                    base[index] = value;
                }
            }
        }

        public override void Insert(int index, int element)
        {
            if (CheckTheElementToNotBreakTheSorting(index, element))
            {
                base.Insert(index, element);
            }
        }


        private bool CheckTheElementToNotBreakTheSorting(int index, int element)
        {
            return index == 0 && base[index] >= element || index > 0 && base[index - 1] <= element && base[index] >= element;
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
            }
        }
    }
}
