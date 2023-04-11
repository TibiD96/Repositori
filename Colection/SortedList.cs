using System;

namespace CollectionData
{
    public class SortedList<T> : List<T> where T: IComparable<T>
    {
        public SortedList()
            : base()
        {
        }

        public override void Add(T element)
        {
            base.Add(element);
            Sorting();
        }
        public override T this[int index]
        {
            set
            {
                if (ElementOrDefault(index - 1, value).CompareTo(value) <= 0 && ElementOrDefault(index + 1, value).CompareTo(value) >= 0)
                {
                    base[index] = value;
                }
            }
        }

        public override void Insert(int index, T element)
        {
            if (ElementOrDefault(index - 1, element).CompareTo(element) <= 0 && ElementOrDefault(index, element).CompareTo(element) >= 0)
            {
                base.Insert(index, element);
            }
        }
        private T ElementOrDefault(int index, T actualValue)
        {
            return index >= 0 && index < Count ? base[index] : actualValue;
        }

        private void Sorting()
        {
            bool sorted = true;
            while (sorted)
            {
                int length = Count;
                sorted = false;
                for (int i = 0; i < length - 1; i++)
                {
                    if (base[i].CompareTo(base[i + 1]) >= 0)
                    {
                        T pivot = base[i];
                        base[i] = base[i + 1];
                        base[i + 1] = pivot;
                        sorted = true;
                    }
                }
            }
        }
    }
}
