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
                T left = ElementOrDefault(index - 1, value);
                T right = ElementOrDefault(index + 1, value);
                if (left.CompareTo(value) <= 0 && right.CompareTo(value) >= 0)
                {
                    base[index] = value;
                }
            }
        }

        public override void Insert(int index, T element)
        {
            T left = ElementOrDefault(index - 1, element);
            T right = ElementOrDefault(index, element);

            if (left.CompareTo(element) <= 0 && right.CompareTo(element) >= 0)
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
