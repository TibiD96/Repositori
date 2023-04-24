using System;
using System.Collections;


namespace CollectionData
{
    public class ReadOnlyArray<T>: IList<T>
    {
        private readonly IList<T> inputArray;

        public ReadOnlyArray(IList<T> inputArray)
        {
            this.inputArray = inputArray;
        }

        public T this[int index] 
        
        { 
            get => inputArray[index]; 
            set => throw new NotSupportedException("Is Read-Only");
        }

        public int Count
        {
            get

            {
                return inputArray.Count;
            }
        }

        public bool IsReadOnly
        {
            get

            {
                return true;
            }
        }

        public void Add(T element)
        {
            throw new NotSupportedException("Is Read-Only");
        }

        public void Clear()
        {
            throw new NotSupportedException("Is Read-Only");
        }

        public bool Contains(T element)
        {
            return inputArray.Contains(element);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            inputArray.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return inputArray.GetEnumerator();
        }

        public int IndexOf(T element)
        {
            return inputArray.IndexOf(element);
        }

        public void Insert(int index, T element)
        {
            throw new NotSupportedException("Is Read-Only");
        }

        public bool Remove(T element)
        {
            throw new NotSupportedException("Is Read-Only");
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Is Read-Only");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return inputArray.GetEnumerator();
        }
    }
}
