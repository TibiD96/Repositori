using System.Security.Cryptography;
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
            dictionary.Add(2, "b");
            dictionary.Add(10, "c");
            dictionary.Add(7, "d");
            dictionary.Add(12, "e");
            Assert.Equal("a", dictionary[1]);
            Assert.Equal("b", dictionary[2]);
            Assert.Equal("c", dictionary[10]);
            Assert.Equal("d", dictionary[7]);
            Assert.Equal("e", dictionary[12]);
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

        public void CheckIfRemoveWorkKeyIsFirstInBucket()
        {
            var dictionary = new Dictionary<int, string>(5);
            var first = new KeyValuePair<int, string>(1, "a");
            var second = new KeyValuePair<int, string>(2, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            var fourth = new KeyValuePair<int, string>(7, "d");
            var fifth = new KeyValuePair<int, string>(12, "d");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            dictionary.Add(fourth);
            dictionary.Add(fifth);
            Assert.False(dictionary.Remove(15));
            Assert.True(dictionary.Remove(12));
        }

        [Fact]

        public void CheckIfRemoveWorkKeyIsNotFirstInBucket()
        {
            var dictionary = new Dictionary<int, string>(5);
            var first = new KeyValuePair<int, string>(1, "a");
            var second = new KeyValuePair<int, string>(2, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            var fourth = new KeyValuePair<int, string>(7, "d");
            var fifth = new KeyValuePair<int, string>(12, "d");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            dictionary.Add(fourth);
            dictionary.Add(fifth);
            Assert.False(dictionary.Remove(15));
            Assert.True(dictionary.Remove(7));
        }

        [Fact]

        public void CheckIfRemovewithPairsWorkIfPairIsFirst()
        {
            var dictionary = new Dictionary<int, string>(5);
            var first = new KeyValuePair<int, string>(1, "a");
            var second = new KeyValuePair<int, string>(2, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            var fourth = new KeyValuePair<int, string>(7, "d");
            var fifth = new KeyValuePair<int, string>(12, "d");
            var sixth = new KeyValuePair<int, string>(15, "d");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            dictionary.Add(fourth);
            dictionary.Add(fifth);
            Assert.False(dictionary.Remove(sixth));
            Assert.True(dictionary.Remove(fifth));
        }

        [Fact]

        public void CheckIfRemovewithPairsWorkIfPairIsNotFirst()
        {
            var dictionary = new Dictionary<int, string>(5);
            var first = new KeyValuePair<int, string>(1, "a");
            var second = new KeyValuePair<int, string>(2, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            var fourth = new KeyValuePair<int, string>(7, "d");
            var fifth = new KeyValuePair<int, string>(12, "d");
            var sixth = new KeyValuePair<int, string>(15, "d");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            dictionary.Add(fourth);
            dictionary.Add(fifth);
            Assert.False(dictionary.Remove(sixth));
            Assert.True(dictionary.Remove(fourth));
        }

        [Fact]

        public void CheckIfCopyToWork()
        {
            var dictionary = new Dictionary<int, string>(5);
            var secondDictionary = new KeyValuePair<int, string>[5];
            var thirdDictionary = new KeyValuePair<int, string>[3];
            var first = new KeyValuePair<int, string>(1, "a");
            var second = new KeyValuePair<int, string>(2, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            var fourth = new KeyValuePair<int, string>(7, "d");
            var fifth = new KeyValuePair<int, string>(12, "d");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            dictionary.Add(fourth);
            dictionary.Add(fifth);
            dictionary.CopyTo(secondDictionary, 0);
            Assert.Equal(5, secondDictionary.Length);
            dictionary.Remove(fourth);
            dictionary.Remove(first);
            dictionary.CopyTo(thirdDictionary, 0);
            Assert.Equal(3, thirdDictionary.Length);
        }

        [Fact]

        public void CheckIfCopyToWorkWhenEmptyPositionsAreNotOrdered()
        {
            var dictionary = new Dictionary<int, string>(5);
            var secondDictionary = new KeyValuePair<int, string>[5];
            var thirdDictionary = new KeyValuePair<int, string>[3];
            var first = new KeyValuePair<int, string>(1, "a");
            var second = new KeyValuePair<int, string>(2, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            var fourth = new KeyValuePair<int, string>(7, "d");
            var fifth = new KeyValuePair<int, string>(12, "d");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            dictionary.Add(fourth);
            dictionary.Add(fifth);
            dictionary.CopyTo(secondDictionary, 0);
            Assert.Equal(5, secondDictionary.Length);
            dictionary.Remove(first);
            dictionary.Remove(fourth);
            dictionary.CopyTo(thirdDictionary, 0);
            Assert.Equal(3, thirdDictionary.Length);
        }

        [Fact]

        public void CheckIfTryGetValueWorkForTrue()
        {
            var dictionary = new Dictionary<int, string>(5);
            var first = new KeyValuePair<int, string>(1, "a");
            var second = new KeyValuePair<int, string>(2, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            var fourth = new KeyValuePair<int, string>(7, "d");
            var fifth = new KeyValuePair<int, string>(12, "d");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            dictionary.Add(fourth);
            dictionary.Add(fifth);
            Assert.True(dictionary.TryGetValue(1, out string value));
            Assert.Equal(first.Value, value);
        }

        [Fact]

        public void CheckIfTryGetValueWorkForFalse()
        {
            var dictionary = new Dictionary<int, string>(5);
            var first = new KeyValuePair<int, string>(1, "a");
            var second = new KeyValuePair<int, string>(2, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            var fourth = new KeyValuePair<int, string>(7, "d");
            var fifth = new KeyValuePair<int, string>(12, "d");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            dictionary.Add(fourth);
            dictionary.Add(fifth);
            Assert.False(dictionary.TryGetValue(3, out string value));
            Assert.Null(value);
        }

        [Fact]

        public void ReusePosition()
        {
            var dictionary = new Dictionary<int, string>(5);
            var first = new KeyValuePair<int, string>(1, "a");
            var second = new KeyValuePair<int, string>(2, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            var fourth = new KeyValuePair<int, string>(7, "d");
            var fifth = new KeyValuePair<int, string>(12, "e");
            var sixth = new KeyValuePair<int, string>(17, "f");
            var seventh = new KeyValuePair<int, string>(3, "g");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            dictionary.Add(fourth);
            dictionary.Add(fifth);
            dictionary.Remove(fourth);
            dictionary.Remove(first);
            dictionary.Add(sixth);
            dictionary.Add(seventh);
            Assert.Equal(dictionary.GetElement(sixth.Key).Key, sixth.Key);
            Assert.Equal(dictionary.GetElement(sixth.Key).Value, sixth.Value);
            Assert.Equal(dictionary.GetElement(seventh.Key).Key, seventh.Key);
            Assert.Equal(dictionary.GetElement(seventh.Key).Value, seventh.Value);
        }

        [Fact]

        public void KeysValues()
        {
            var dictionary = new Dictionary<int, string>(5);
            var first = new KeyValuePair<int, string>(1, "a");
            var second = new KeyValuePair<int, string>(2, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            var fourth = new KeyValuePair<int, string>(7, "d");
            var fifth = new KeyValuePair<int, string>(12, "e");
            var sixth = new KeyValuePair<int, string>(17, "f");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            dictionary.Add(fourth);
            dictionary.Add(fifth);
            Assert.True(dictionary.Keys.Contains(7));
            dictionary.Remove(fourth);
            dictionary.Remove(first);
            Assert.Equal(3, dictionary.Keys.Count);
            Assert.False(dictionary.Keys.Contains(7));
            Assert.False(dictionary.Keys.Contains(1));
            dictionary.Add(sixth);
            Assert.False(dictionary.Keys.Contains(7));
            Assert.True(dictionary.Keys.Contains(17));
            Assert.Equal(4, dictionary.Keys.Count);
        }

        [Fact]

        public void KeysValuesEmptyPositionNotInOrder()
        {
            var dictionary = new Dictionary<int, string>(5);
            var first = new KeyValuePair<int, string>(1, "a");
            var second = new KeyValuePair<int, string>(2, "b");
            var third = new KeyValuePair<int, string>(10, "c");
            var fourth = new KeyValuePair<int, string>(7, "d");
            var fifth = new KeyValuePair<int, string>(12, "e");
            var sixth = new KeyValuePair<int, string>(17, "f");
            dictionary.Add(first);
            dictionary.Add(second);
            dictionary.Add(third);
            dictionary.Add(fourth);
            dictionary.Add(fifth);
            Assert.True(dictionary.Keys.Contains(7));
            dictionary.Remove(first);
            dictionary.Remove(fourth);
            Assert.Equal(3, dictionary.Keys.Count);
            Assert.False(dictionary.Keys.Contains(7));
            Assert.False(dictionary.Keys.Contains(1));
            dictionary.Add(sixth);
            Assert.False(dictionary.Keys.Contains(7));
            Assert.True(dictionary.Keys.Contains(17));
            Assert.Equal(4, dictionary.Keys.Count);
        }

        [Fact]
        public void GetEnumeratorReturnsAllKeyValuePairs()
        {
            var dictionary = new Dictionary<int, string>(5);
            dictionary.Add(1, "a");
            dictionary.Add(2, "b");
            dictionary.Add(10, "c");
            dictionary.Add(7, "d");
            dictionary.Add(12, "e");
            dictionary.Remove(7);
            dictionary.Remove(1);
            var enumerator = dictionary.GetEnumerator();
            var result = new List<KeyValuePair<int, string>>();
            while (enumerator.MoveNext())
            {
                result.Add(enumerator.Current);
            }

            Assert.Equal(3, result.Count);
            Assert.Contains(new KeyValuePair<int, string>(2, "b"), result);
            Assert.Contains(new KeyValuePair<int, string>(10, "c"), result);
            Assert.Contains(new KeyValuePair<int, string>(12, "e"), result);
        }
    }
}
