using Xunit;

namespace CollectionData
{
    public class ListTests
    {
        [Fact]

        public void CheckIfConstructorWorks()
        {
            var list = new List<int>() { 1};
            Assert.Equal(1, list[0]);
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.Equal(0, list[1]));
            Assert.Throws<ArgumentOutOfRangeException>(() => Assert.Equal(0, list[2]));

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

        [Fact]

        public void VerrifieTheCopyToMethod()
        {
            var firstList = new List<int>(){ 1, 2, 3, 5};
            var secondList = new int[4];
            firstList.CopyTo(secondList, 0);
            Assert.Equal(firstList, secondList);
        }

        [Fact]
        public void CopyToMethodWhenSecondListIsNUll()
        {
            var firstList = new List<int>() { 1, 2, 3, 5 };
            int[] secondList = null;
            Assert.Throws<ArgumentNullException>(() => firstList.CopyTo(secondList, 0));
        }

        [Fact]
        public void CopyToMethodWhenIndexIsNegative()
        {
            var firstList = new List<int>();
            int[] secondList = new int[5];
            Assert.Throws<ArgumentOutOfRangeException>(() => firstList.CopyTo(secondList, -1));
        }

        [Fact]
        public void CopyToMethodWhenArgumentException()
        {
            var firstList = new List<int>() { 1, 2, 3};
            int[] secondList = { 4, 5, 6, 7, 8 };
            Assert.Throws<ArgumentException>(() => firstList.CopyTo(secondList, 3));
        }

        [Fact]
        public void InsertOutOfRangeExpIndexIsNegative()
        {
            var list = new List<int>();
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(-2, 3));
        }

        [Fact]

        public void InsertOutOfRangeExpIndexBiggerThanArrayLength()
        {
            var list = new List<int>();
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            list.Add(8);
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(6, 100));
        }

        [Fact]

        public void RemoveAtOutOfRangeExp()
        {
            var list = new List<int>();
            int givenIndex = 6;
            list.Add(2);
            list.Add(5);
            list.Add(10);
            list.Add(11);
            list.Add(8);
            Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(givenIndex));

        }

        [Fact]
        public void ReturnTrueIfReadOnly()
        {
            var list = new List<int>();
            list = list.ReadOnly();
            Assert.True(list.IsReadOnly);
        }

        [Fact]
        public void ReturnFalseIfNotReadOnly()
        {
            var list = new List<int>();
            Assert.False(list.IsReadOnly);
        }

        [Fact]
        public void ReturnTrueIfReadOnlyArrayIsNotChanged()
        {
            var list = new List<int>() { 1, 2, 3, 4};
            var originalList = list;
            list = list.ReadOnly();
            Assert.True(list.IsReadOnly);
            Assert.Equal(originalList, list);
        }

        [Fact]
        public void InsertIsReadonlyException()
        {
            var list = new List<int> { 1, 2, 3 };
            list = list.ReadOnly();
            Assert.Throws<NotSupportedException>(() => list.Insert(1, 2));
        }

        [Fact]
        public void AddIsReadonlyException()
        {
            var list = new List<int>();
            list = list.ReadOnly();
            Assert.Throws<NotSupportedException>(() => list.Add(1));
        }

        [Fact]
        public void ClearIsReadonlyException()
        {
            var list = new List<int>() { 1, 2, 3};
            list = list.ReadOnly();
            Assert.Throws<NotSupportedException>(() => list.Clear());
        }

        [Fact]
        public void SetIsReadonlyException()
        {
            var list = new List<int>();
            list = list.ReadOnly();
            Assert.Throws<NotSupportedException>(() => list[0] = 1);
        }

        [Fact]
        public void GetOutOfRangeExp()
        {
            var list = new List<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1] == 1);
        }

        [Fact]
        public void SetOutOfRangeExp()
        {
            var list = new List<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1] = 1);
        }
    }
}