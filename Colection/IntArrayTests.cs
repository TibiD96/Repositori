using Xunit;

namespace CollectionData
{
    public class IntArrayTests
    {
        [Fact]

        public void AddElemetsInArrayPositioningItOnTheEnd()
        {
            var input = new IntArray();
            input.Add(5);
            input.Add(10);
            input.Add(100);
            input.Add(25);
            Assert.Equal(4, input.Count());
            Assert.Equal(25, input.Element(input.Count() - 1));
        }

        [Fact]

        public void ChangeValueOfTheElementFromGivenIndex()
        {
            var input = new IntArray();
            int correctValueAfterChanging = 1000;
            input.Add(5);
            input.Add(10);
            input.Add(100);
            input.Add(25);
            input.SetElement(0, 1000);
            Assert.Equal(input.Element(0), correctValueAfterChanging);
        }

        [Fact]

        public void ReturnTrueIfElementIsInArray()
        {
            var input = new IntArray();
            int numbertoCheck = 1000;
            input.Add(5);
            input.Add(10);
            input.Add(1000);
            input.Add(25);
            Assert.True(input.Contains(numbertoCheck));
        }

        [Fact]

        public void ReturnFalseIfElementIsNotInArray()
        {
            var input = new IntArray();
            int numbertoCheck = 1000;
            input.Add(5);
            input.Add(10);
            input.Add(30);
            input.Add(25);
            Assert.False(input.Contains(numbertoCheck));
        }

        [Fact]

        public void ReturnTheIndexOfGivenNumberElseReturnMinusOne()
        {
            var input = new IntArray();
            input.Add(5);
            input.Add(10);
            input.Add(40);
            input.Add(25);
            Assert.Equal(2, input.IndexOf(40));
            Assert.Equal(-1, input.IndexOf(100));
        }

        [Fact]

        public void InsertTheNumberToTheSpecifiedPosition()
        {
            var input = new IntArray();
            input.Add(0);
            input.Add(1);
            input.Add(2);
            input.Add(3);
            input.Insert(2, 3);
            Assert.Equal(3, input.Element(2));
            Assert.Equal(2, input.Element(3));
        }

        [Fact]

        public void DeleteAllElementsFfromArray()
        {
            var input = new IntArray();
            input.Add(0);
            input.Add(1);
            input.Add(2);
            input.Add(3);
            input.Clear();
            Assert.Equal(0, input.Count());
        }
    }
}