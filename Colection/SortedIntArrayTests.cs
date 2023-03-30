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
            int[] correctOrder = { 3, 5 };
            Assert.Equal(correctOrder, input);
        }
    }
}
