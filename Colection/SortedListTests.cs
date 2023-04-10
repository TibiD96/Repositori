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
    }
}