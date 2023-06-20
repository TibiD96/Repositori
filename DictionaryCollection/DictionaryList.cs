using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DictionaryCollection
{
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly int[] buckets;

        private readonly Item<TKey, TValue>[] items;

        private readonly int dimension = 5;

        private int passNextProperty = -1;

        public Dictionary(int dimension)
        {
            this.buckets = new int[dimension];
            this.items = new Item<TKey, TValue>[dimension];
            for (int i = 0; i < dimension; i++)
            {
                this.buckets[i] = -1;
            }
        }

        public ICollection<TKey> Keys => new List<TKey>();

        public ICollection<TValue> Values => new List<TValue>();

        public int Count { get; set; }

        public bool IsReadOnly { get; set; }

        public TValue this[TKey key]
        {
            get
            {
                if (FindKey(key) == -1)
                {
                    throw new KeyNotFoundException();
                }

                return items[FindKey(key)].Value;
            }

            set
            {
              items[buckets[BucketChooser(key)]].Value = value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            int bucketNumber = BucketChooser(key);
            var item = new Item<TKey, TValue>();
            item.Key = key;
            item.Value = value;
            Keys.Add(key);
            Values.Add(value);
            items[Count] = item;
            item.Next = buckets[bucketNumber];
            buckets[bucketNumber] = Count;
            Count++;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Count = 0;
            for (int i = 0; i < dimension; i++)
            {
                this.buckets[i] = -1;
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            int keyPosition = FindKey(item.Key);
            if (keyPosition == -1)
            {
                return false;
            }

            return items[keyPosition].Value.Equals(item.Value) && ContainsKey(item.Key);
        }

        public bool ContainsKey(TKey key)
        {
            return FindKey(key) != -1;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            for (int i = 0; i < Count; i++)
            {
                array[arrayIndex + i] = new KeyValuePair<TKey, TValue>(items[i].Key, items[i].Value);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            int bucketNumber = BucketChooser(key);
            int keyPosition = FindKey(key);

            if (keyPosition == -1)
            {
                return false;
            }

            if (items[buckets[bucketNumber]].Key.Equals(key))
            {
                buckets[bucketNumber] = items[keyPosition].Next;
                items[keyPosition] = default;
                Count--;
                return true;
            }

            items[passNextProperty].Next = items[keyPosition].Next;
            items[keyPosition] = default;
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

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int BucketChooser(TKey key)
        {
            const int numberOfBuckets = 5;
            return Math.Abs(key.GetHashCode() % numberOfBuckets);
        }

        private int FindKey(TKey key)
        {
            int bucketNumber = BucketChooser(key);
            int index = buckets[bucketNumber];
            while (index != -1)
            {
                if (items[index].Key.Equals(key))
                {
                    return index;
                }

                passNextProperty = index;
                index = items[index].Next;
            }

            return -1;
        }
    }
}
