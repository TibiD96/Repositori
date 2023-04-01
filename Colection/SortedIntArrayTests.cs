using Xunit;

namespace CollectionData
{
    public class SortedIntArrayTests
    {
        [Fact]

        public void SorttedCorrectForAShortList()
        {
            var input = new SortedIntArray();
            input.Add(5);
            input.Add(3);
            string correctOrder = input[0].ToString() + input[1].ToString();
            Assert.Equal("35", correctOrder);
        }

        [Fact]

        public void SorttedCorrectForALongertList()
        {
            var input = new SortedIntArray();
            input.Add(5);
            input.Add(2000);
            input.Add(100);
            input.Add(50);
            input.Add(10);
            string correctOrder = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString() + input[4].ToString();
            Assert.Equal("510501002000", correctOrder);
        }

        [Fact]

        public void CorrectInsertAndSortIsNotBroken()
        {
            var input = new SortedIntArray();
            input.Add(5);
            input.Add(2000);
            input.Add(50);
            input.Add(10);
            input.Insert(3, 500);
            string correctOrder = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString() + input[4].ToString();
            Assert.Equal("510505002000", correctOrder);
        }

        [Fact]

        public void DontInsertIfTheSortIsBroken()
        {
            var input = new SortedIntArray();
            input.Add(5);
            input.Add(2000);
            input.Add(100);
            input.Add(50);
            input.Add(10);
            input.Insert(3, 500);
            string correctOrder = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString() + input[4].ToString();
            Assert.Equal("510501002000", correctOrder);
        }

        [Fact]

        public void InsertOnFirstPosition()
        {
            var input = new SortedIntArray();
            input.Add(5);
            input.Add(2000);
            input.Add(100);
            input.Add(50);
            input.Add(10);
            input.Insert(0, 3);
            string correctOrder = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString() + input[4].ToString() +input[5].ToString();
            Assert.Equal("510501002000", correctOrder);
        }

        [Fact]

        public void SorttedCorrectAfterRemovingElement()
        {
            var input = new SortedIntArray();
            input.Add(5);
            input.Add(2000);
            input.Add(100);
            input.Remove(100);
            string correctOrder = input[0].ToString() + input[1].ToString();
            Assert.Equal("52000", correctOrder);
        }

        [Fact]

        public void SorttedCorrectAfterRemovingElementLongerArray()
        {
            var input = new SortedIntArray();
            input.Add(5);
            input.Add(2000);
            input.Add(100);
            input.Add(10);
            input.Add(500);
            input.Add(4000);
            input.Remove(100);
            string correctOrder = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString() + input[4].ToString();
            Assert.Equal("51050020004000", correctOrder);
        }
    }
}
