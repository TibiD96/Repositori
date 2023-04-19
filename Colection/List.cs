using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Reflection;

namespace CollectionData
{
    public class List<T> : IList<T>
    {
        private T[] inputArray;

        public List()
        {
            this.inputArray = new T[4];
        }

        public int Count { get; private set; }

        public bool IsReadOnly { get; }

        public virtual T this[int index]
        {
            get => inputArray[index];
            set => inputArray[index] = value;
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

        public void CopyTo(T[] array, int arrayIndex)
        {
            ArgumentNullException(array);
            ArgumentOutOfRangeException(arrayIndex);
            for (int i = 0; i < inputArray.Length; i++)
            {
              array[arrayIndex + i] = inputArray[i];
            }
            
        }

        public virtual void Add(T element)
        {
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
            ArgumentOutOfRangeException(index);
            Resizing();
            ShiftRight(index);
            inputArray[index] = element;
            Count++;
        }

        public void Clear()
        {
            Count = 0;
        }

        public bool Remove(T element)
        {
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

        public void ArgumentNullException(T[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("can't be null");
            }

            return;
        }

        public void ArgumentOutOfRangeException(int index)
        {
            if (index >= Count || index < 0)
            {
                throw new ArgumentOutOfRangeException("ivalid index");
            }

            return;
        }
    }
}
