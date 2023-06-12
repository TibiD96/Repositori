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
    }
}
