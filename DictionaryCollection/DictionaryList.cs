using System;
using System.Collections;
using System.Reflection;

namespace DictionaryCollection
{
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Item<TKey, TValue>[] items;

        private readonly int[] buckets;

        private int freeIndex = -1;

        public Dictionary(int dimension)
        {
            this.buckets = new int[dimension];
            this.items = new Item<TKey, TValue>[dimension];
            Array.Fill(this.buckets, -1);
        }

        public ICollection<TKey> Keys
        {
            get
            {
                var keys = new List<TKey>();
                foreach (var item in this)
                {
                  keys.Add(item.Key);
                }

                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                var values = new List<TValue>();
                foreach (var item in this)
                {
                    values.Add(item.Value);
                }

                return values;
            }
        }

        public int Count { get; set; }

        public bool IsReadOnly { get; set; }

        public TValue this[TKey key]
        {
            get
            {
                int keyPosition = FindKey(key);
                if (keyPosition == -1)
                {
                    throw new KeyNotFoundException();
                }

                return items[keyPosition].Value;
            }

            set
            {
                int keyPosition = FindKey(key);
                if (keyPosition == -1)
                {
                    throw new KeyNotFoundException();
                }

                items[keyPosition].Value = value;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            KeyValuePair<TKey, TValue>[] components = new KeyValuePair<TKey, TValue>[Count];
            for (int i = 0; i < buckets.Length; i++)
            {
                for (int index = buckets[i]; index != -1; index = items[index].Next)
                {
                    components[i] = new KeyValuePair<TKey, TValue>(items[index].Key, items[index].Value);
                    yield return components[i];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TKey key, TValue value)
        {
            int indexWhereToAdd;
            if (key == null)
            {
                throw new ArgumentNullException("key can't be null");
            }

            if (FindKey(key) != -1)
            {
                throw new ArgumentException("Key allready Exist");
            }

            int bucketNumber = BucketChooser(key);
            var item = new Item<TKey, TValue>();
            item.Key = key;
            item.Value = value;
            item.Next = buckets[bucketNumber];
            if (freeIndex == -1)
            {
                indexWhereToAdd = Count;
                buckets[bucketNumber] = Count;
            }
            else
            {
                int newValueFreeIndex = items[freeIndex].Next;
                indexWhereToAdd = freeIndex;
                buckets[bucketNumber] = freeIndex;
                freeIndex = newValueFreeIndex;
            }

            items[indexWhereToAdd] = item;
            Count++;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Count = 0;
            Array.Fill(this.buckets, -1);
            Array.Fill(items, null);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            int keyPosition = FindKey(item.Key);
            if (keyPosition == -1)
            {
                return false;
            }

            return items[keyPosition].Value.Equals(item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            return FindKey(key) != -1;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            int index = 0;
            if (array == null)
            {
                throw new ArgumentNullException("Array can't be null");
            }

            if (arrayIndex < 0 || arrayIndex > array.Length - 1)
            {
                throw new ArgumentOutOfRangeException("ArgumentOutOfRange");
            }

            for (int i = 0; i < items.Length; i++)
            {
                int freeElement = freeIndex;
                while (freeElement != -1)
                {
                    if (freeElement == i)
                    {
                        break;
                    }

                    freeElement = items[freeElement].Next;
                }

                if (freeElement != i)
                {
                    array[arrayIndex + index] = new KeyValuePair<TKey, TValue>(items[i].Key, items[i].Value);
                    index++;
                }
            }
        }

        public bool Remove(TKey key)
        {
            int keyPosition = FindKeyPositionAndIndexItemBefore(key, out int previousElement);
            int bucketNumber = BucketChooser(key);

            if (keyPosition == -1)
            {
                return false;
            }

            if (items[buckets[bucketNumber]].Key.Equals(key))
            {
                buckets[bucketNumber] = items[keyPosition].Next;
            }
            else
            {
                items[previousElement].Next = items[keyPosition].Next;
            }

            items[keyPosition].Key = default;
            items[keyPosition].Value = default;
            items[keyPosition].Next = freeIndex;
            freeIndex = keyPosition;
            Count--;

            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            int keyPosition = FindKey(item.Key);
            if (keyPosition == -1)
            {
                return false;
            }

            return items[keyPosition].Value.Equals(item.Value) && Remove(item.Key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int keyPosition = FindKey(key);
            if (keyPosition != -1)
            {
                value = items[keyPosition].Value;
                return true;
            }

            value = default;
            return false;
        }

        public int BucketChooser(TKey key)
        {
            return Math.Abs(key.GetHashCode() % 5);
        }

        public Item<TKey, TValue> GetElement(TKey key)
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Dictionary is empty.");
            }

            return items[FindKey(key)];
        }

        private int FindKey(TKey key)
        {
            return FindKeyPositionAndIndexItemBefore(key, out _);
        }

        private int FindKeyPositionAndIndexItemBefore(TKey key, out int indexItemBeforeItemsKey)
        {
            indexItemBeforeItemsKey = -1;
            if (key == null)
            {
                throw new ArgumentNullException("key can't be null");
            }

            for (int i = 0; i < buckets.Length; i++)
            {
                for (int index = buckets[i]; index != -1; index = items[index].Next)
                {
                    if (items[index].Key.Equals(key))
                    {
                        return index;
                    }

                    indexItemBeforeItemsKey = index;
                }
            }

            return -1;
        }
    }
}
