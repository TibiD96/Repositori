using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DictionaryCollection
{
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly int[] buckets;

        private readonly Item<TKey, TValue>[] items;

        public Dictionary(int dimension)
        {
            this.buckets = new int[dimension];
            this.items = new Item<TKey, TValue>[dimension];
        }

        public ICollection<TKey> Keys => new List<TKey>();

        public ICollection<TValue> Values => new List<TValue>();

        public int Count { get; set; }

        public bool IsReadOnly { get; set; }

        public TValue this[TKey key]
        {
            get
            {
                return items[buckets[BucketChooser(key)]].Value;
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
            items[Count] = item;
            buckets[bucketNumber] = Count;
            Keys.Add(key);
            Values.Add(value);
            Count++;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Count = 0;
            buckets.GetLength(0);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
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
