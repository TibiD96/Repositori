using Xunit;

namespace CollectionData
{
    public class IntArrayTests
    {
        [Fact]

        public void ReturnNullForNullAndEmptyForEmpty()
        {
            var input = new IntArray();
            input.Add(5);
            input.Add(10);
            input.Add(100);
            input.Add(25);
            Assert.Equal(4, input.Count());
            Assert.Equal(25, input.Element(input.Count() - 1));
        }
    }
}