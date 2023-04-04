using System;

namespace CollectionData
{
    public class ObjectArray
    {
        private object[] input;

        public ObjectArray()
        {
            this.input = new object[4];
        }

        public int Count { get; private set; }

        public object this[int index]
        {
            get => input[index];
            set => input[index] = value;
        }

        public void Add(object element)
        {
            Resizing();
            input[Count++] = element;
        }

        public bool Contains(object element)
        {
            return IndexOf(element) > -1;
        }

        public int IndexOf(object element)
        {
            for (int i = 0; i < Count; i++)
            {
                if (input[i].Equals(element))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, object element)
        {
            Resizing();
            ShiftRight(index);
            input[index] = element;
            Count++;
        }

        public void Clear()
        {
            Count = 0;
        }

        private void ShiftRight(int index)
        {
            for (int i = Count - 1; i >= index; i--)
            {
                input[i + 1] = input[i];
            }
        }

        private void Resizing()
        {
            if (Count == input.Length)
            {
                Array.Resize(ref input, input.Length * 2);
            }
        }
    }
}
