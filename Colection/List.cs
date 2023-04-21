using System;
using System.Collections;

namespace CollectionData
{
    public class List<T> : IList<T>
    {
        private T[] inputArray;

        public List()
        {
            this.inputArray = new T[4];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return inputArray[i];
            }
        }

        public int Count { get; private set; }

        public bool IsReadOnly { get; private set; }

        public virtual T this[int index]
        {
            get
            {
                ArgumentOutOfRangeException(index);
                return inputArray[index];
            }
            set
            {
                NotSupportedException();
                ArgumentOutOfRangeException(index);
                inputArray[index] = value;
            }
        }

        public List<T> ReadOnly()
        {
            List<T> readOnlyArray = new List<T>();

            foreach (var element in this)
            {
                readOnlyArray.Add(element);
            }

            readOnlyArray.IsReadOnly = true;
            return readOnlyArray;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ArgumentNullException(array);
            ArgumentException(array, arrayIndex);
            ArgumentOutOfRangeException(arrayIndex);

            for (int i = 0; i < Count; i++)
            {
              array[arrayIndex + i] = inputArray[i];
            }
            
        }

        public virtual void Add(T element)
        {
            NotSupportedException();
            Resizing();
            inputArray[Count++] = element;
        }

        public bool Contains(T element)
        {
            return IndexOf(element) > -1;
        }

        public virtual int IndexOf(T element)
        {
            for (int i = 0; i < Count; i++)
            {
                if (inputArray[i].Equals(element))
                {
                    return i;
                }
            }

            return -1;
        }

        public virtual void Insert(int index, T element)
        {
            NotSupportedException();
            ArgumentOutOfRangeException(index);
            Resizing();
            ShiftRight(index);
            inputArray[index] = element;
            Count++;
        }

        public void Clear()
        {
            NotSupportedException();
            Count = 0;
        }

        public bool Remove(T element)
        {
            NotSupportedException();
            int index = IndexOf(element);
            
            if (index == -1)
            {
                return false;
            }

            RemoveAt(index);

            return true;
        }

        public void RemoveAt(int index)
        {
            NotSupportedException();
            ArgumentOutOfRangeException(index);
            ShiftLeft(index);
            Count--;
        }

        private void ShiftRight(int index)
        {
            for (int i = Count - 1; i >= index; i--)
            {
                inputArray[i + 1] = inputArray[i];
            }
        }

        private void ShiftLeft(int index)
        {
            for (int i = index; i <= Count - 1; i++)
            {
                inputArray[i] = inputArray[i + 1];
            }
        }

        private void Resizing()
        {
            if (Count == inputArray.Length)
            {
                Array.Resize(ref inputArray, inputArray.Length * 2);
            }
        }

        private void ArgumentNullException(T[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("can't be null");
            }

            return;
        }

        private void ArgumentOutOfRangeException(int index)
        {
            if (index >= Count || index < 0)
            {
                throw new ArgumentOutOfRangeException("ivalid index");
            }

            return;
        }

        private void NotSupportedException()
        {
            if (IsReadOnly)
            {
                throw new NotSupportedException("Is Read-Only");
            }

            return;
        }

        private void ArgumentException(T[] array, int index)
        {
            if (array.Length - index < Count)
            {
                throw new ArgumentException("No space");
            }

            return;
        }
    }
}
