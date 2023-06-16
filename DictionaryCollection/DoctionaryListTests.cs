using Newtonsoft.Json.Linq;
using Xunit;

namespace DictionaryCollection
{
    public class DoctionaryListTests
    {
        [Fact]

        public void AddBasedOnKeyAndValueWork()
        {
            var dictionary = new Dictionary<int, string>(5);
            dictionary.Add(1, "a");
            dictionary.Add(10, "b");
            dictionary.Add(3, "c");
            dictionary.Add(7, "d");
            Assert.Equal("a", dictionary[1]);
            Assert.Equal("b", dictionary[10]);
            Assert.Equal("c", dictionary[3]);
            Assert.Equal("d", dictionary[7]);
        }

        [Fact]

        public void CheckIfElementsAreInCorrectBucket()
        {
            var dictionary = new Dictionary<int, string>(5);
            dictionary.Add(1, "a");
            dictionary.Add(10, "b");
            dictionary.Add(3, "c");
            dictionary.Add(7, "d");
            Assert.Equal(1, dictionary.BucketChooser(1));
            Assert.Equal(0, dictionary.BucketChooser(10));
            Assert.Equal(2, dictionary.BucketChooser(2));
            Assert.Equal(2, dictionary.BucketChooser(7));
        }

        [Fact]

        public void AddBasedOnPairWork()
        {
            var dictionary = new Dictionary<int, string>(5);
            var itemToAdd = new KeyValuePair<int, string>(12, "s");
            dictionary.Add(itemToAdd);
            Assert.Equal("s", dictionary[12]);
        }

        [Fact]

        public void ClearMethodeWork()
        {
            var dictionary = new Dictionary<int, string>(5);
            var itemToAdd = new KeyValuePair<int, string>(12, "s");
            dictionary.Add(itemToAdd);
            dictionary.Clear();
            Assert.Equal(0, dictionary.Count);
        }

        [Fact]

        public void CheckIfContainKeyWork()
        {
            var dictionary = new Dictionary<int, string>(5);
            dictionary.Add(7, "d");
            dictionary.Add(12, "c");
            dictionary.Add(1, "a");
            dictionary.Add(10, "b");
            dictionary.Add(3, "c");
            Assert.True(dictionary.ContainsKey(7));
            Assert.True(dictionary.ContainsKey(12));
            Assert.True(dictionary.ContainsKey(1));
            Assert.True(dictionary.ContainsKey(10));
            Assert.True(dictionary.ContainsKey(3));
        }

        [Fact]

        public void CheckIfContainWork()
        {
            var dictionary = new Dictionary<int, string>(5);
            var first = new KeyValuePair<int, string>(12, "a");
            var second = new KeyValuePair<int, string>(7, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            Assert.True(dictionary.Contains(first));
            Assert.True(dictionary.Contains(second));
            Assert.True(dictionary.Contains(third));
        }

        [Fact]

        public void CheckIfRemoveWork()
        {
            var dictionary = new Dictionary<int, string>(5);
            var first = new KeyValuePair<int, string>(12, "a");
            var second = new KeyValuePair<int, string>(5, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            Assert.False(dictionary.Remove(15));
            Assert.True(dictionary.Remove(12));
        }
    }
}
