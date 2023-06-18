using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DictionaryCollection
{
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly int[] buckets;

        private readonly Item<TKey, TValue>[] items;

        private readonly int dimension = 5;

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
                for (int i = buckets[BucketChooser(key)]; i >= 0; i--)
                {
                    if (items[i].Key.Equals(key))
                    {
                        return items[i].Value;
                    }
                }

                throw new KeyNotFoundException();
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
            for (int i = 0; i < Count; i++)
            {
                if (items[i].Key.Equals(item.Key) && items[i].Value.Equals(item.Value))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsKey(TKey key)
        {
            for (int i = 0; i < Count; i++)
            {
                if (items[i].Key.Equals(key))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            int bucketNumber = BucketChooser(key);
            int index = buckets[bucketNumber];
            if (!ContainsKey(key))
            {
                return false;
            }

            if (items[buckets[bucketNumber]].Key.Equals(key))
            {
                buckets[bucketNumber] = items[index].Next;
                items[index] = default;
                Count--;
                return true;
            }

            while (items[index].Next != -1)
            {
                if (items[index].Key.Equals(key))
                {
                    items[buckets[bucketNumber]].Next = items[index].Next;
                    items[index] = default;
                    Count--;
                    return true;
                }

                index--;
            }

            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            int bucketNumber = BucketChooser(item.Key);
            int index = buckets[bucketNumber];
            if (!ContainsKey(item.Key))
            {
                return false;
            }

            if (items[buckets[bucketNumber]].Key.Equals(item.Key) && items[buckets[bucketNumber]].Value.Equals(item.Value))
            {
                buckets[bucketNumber] = items[index].Next;
                items[index] = default;
                Count--;
                return true;
            }

            while (items[index].Next != -1)
            {
                if (items[buckets[bucketNumber]].Key.Equals(item.Key) && items[buckets[bucketNumber]].Value.Equals(item.Value))
                {
                    items[buckets[bucketNumber]].Next = items[index].Next;
                    items[index] = default;
                    Count--;
                    return true;
                }

                index--;
            }

            return false;
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
    }
}
