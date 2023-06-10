namespace DictionaryCollection
{
    public class Item<TKey, TValue>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public int Next { get; set; }
    }
}