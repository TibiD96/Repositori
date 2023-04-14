using Xunit;

namespace CollectionData
{
    public class ListTests
    {
        [Fact]

        public void CheckIfConstructorWorks()
        {
            var list = new List<int>();
            Assert.Equal(0, list.Count);
            Assert.Equal(0, list[0]);
            Assert.Equal(0, list[1]);
            Assert.Equal(0, list[2]);

        }

        [Fact]

        public void CheckIfAddMethodeWorks()
        {
            var list = new List<int>();
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            list.Add(8);
            Assert.Equal(2, list[0]);
            Assert.Equal(5, list[1]);
            Assert.Equal(10, list[2]);
            Assert.Equal(11, list[3]);
            Assert.Equal(8, list[4]);
        }

        [Fact]

        public void ReturnTrueIfElementIsContained()
        {
            var list = new List<int>();
            int elementToCheck = 10;
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            list.Add(8);
            Assert.True(list.Contains(elementToCheck));
        }

        [Fact]

        public void ReturnFalseIfElementIsContained()
        {
            var list = new List<int>();
            int elementToCheck = 100;
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            list.Add(8);
            Assert.False(list.Contains(elementToCheck));
        }

        [Fact]

        public void ReturnTheIndexOfGivenNumberElseReturnMinusOne()
        {
            var list = new List<int>();
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            list.Add(8);
            Assert.Equal(2, list.IndexOf(10));
            Assert.Equal(-1, list.IndexOf(100));
        }

        [Fact]

        public void InsertTheNumberToTheSpecifiedPosition()
        {
            var list = new List<int>();
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            list.Add(8);
            list.Insert(2, 100);
            Assert.Equal(100, list[2]);
            Assert.Equal(10, list[3]);
        }

        [Fact]

        public void DeleteAllElementsFfromArray()
        {
            var list = new List<int>();
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            list.Add(8);
            list.Clear();
            Assert.Equal(0, list.Count);
        }

        [Fact]

        public void DeleteTheFirstAppearenceOfTheGivenElement()
        {
            var list = new List<int>();
            int givenElelement = 10;
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            list.Add(8);
            Assert.True(list.Remove(givenElelement));
            Assert.Equal(4, list.Count);
            Assert.Equal(11, list[2]);
        }

        [Fact]

        public void DeleteTheElementFromTheGivenIndex()
        {
            var list = new List<int>();
            int givenIndex = 3;
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            list.Add(8);
            list.RemoveAt(givenIndex);
            Assert.Equal(4, list.Count);
            Assert.Equal(8, list[3]);

        }

        [Fact]

        public void ReturnFalseIfTheGivenElementForRemoveIsNotPartOfTheArray()
        {
            var list = new List<int>();
            int givenElelement = 80;
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            list.Add(8);
            Assert.False(list.Remove(givenElelement));
            Assert.Equal(5, list.Count);
        }
    }
}