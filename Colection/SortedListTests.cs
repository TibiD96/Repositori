using Xunit;

namespace CollectionData
{
    public class SortedListTests
    {
        [Fact]

        public void SorttedCorrectForAShortList()
        {
            var input = new SortedList<int>();
            input.Add(5);
            input.Add(3);
            string correctOrder = input[0].ToString() + input[1].ToString();
            Assert.Equal("35", correctOrder);
        }

        [Fact]

        public void SorttedCorrectForALongertList()
        {
            var input = new SortedList<int>();
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
            var input = new SortedList<int>();
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
            var input = new SortedList<int>();
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

        public void DontInsertOnFirstPositionIfTheSortIsBroken()
        {
            var input = new SortedList<int>();
            input.Add(5);
            input.Add(2000);
            input.Add(100);
            input.Add(50);
            input.Add(10);
            input.Insert(0, 500);
            string correctOrder = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString() + input[4].ToString();
            Assert.Equal("510501002000", correctOrder);
        }

        [Fact]

        public void InsertOnFirstPosition()
        {
            var input = new SortedList<int>();
            input.Add(5);
            input.Add(2000);
            input.Add(100);
            input.Add(50);
            input.Add(10);
            input.Insert(0, 3);
            string correctOrder = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString() + input[4].ToString() + input[5].ToString();
            Assert.Equal("3510501002000", correctOrder);
        }

        [Fact]

        public void InsertOnFirstPositionTheSameNumber()
        {
            var input = new SortedList<int>();
            input.Add(5);
            input.Add(2000);
            input.Add(100);
            input.Add(50);
            input.Add(10);
            input.Insert(0, 5);
            string correctOrder = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString() + input[4].ToString() + input[5].ToString();
            Assert.Equal("5510501002000", correctOrder);
        }


        [Fact]

        public void SorttedCorrectAfterRemovingElement()
        {
            var input = new SortedList<int>();
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
            var input = new SortedList<int>();
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

        [Fact]

        public void CanModifyAElementIfSortingIsNotAffected()
        {
            var input = new SortedList<int>();
            input.Add(5);
            input.Add(2000);
            input.Add(10);
            input.Add(500);
            input.Add(4000);
            input[0] = 3;
            string correctOrder = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString() + input[4].ToString();
            Assert.Equal("31050020004000", correctOrder);
        }

        [Fact]

        public void CantModifyAElementIfSortingIsAffected()
        {
            var input = new SortedList<int>();
            input.Add(5);
            input.Add(2000);
            input.Add(6);
            input.Add(500);
            input.Add(4000);
            input[0] = 8;
            string correctOrder = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString() + input[4].ToString();
            Assert.Equal("5650020004000", correctOrder);
        }

        [Fact]
        public void SetMoreThanOneElement()
        {
            var input = new SortedList<int>();
            input.Add(5);
            input.Add(9);
            input.Add(4);
            input.Add(10);
            input[0] = 2;
            input[1] = 6;
            string result = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString();
            Assert.Equal("26910", result);
        }

        [Fact]
        public void SetTheFirstAndLastElement()
        {
            var input = new SortedList<int>();
            input.Add(3);
            input.Add(5);
            input.Add(20);
            input.Add(15);
            input[0] = 2;
            input[3] = 40;
            string result = input[0].ToString() + input[1].ToString() + input[2].ToString() + input[3].ToString();
            Assert.Equal("251540", result);
        }
    }
}