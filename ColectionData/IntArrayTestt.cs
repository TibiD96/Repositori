using Xunit;

namespace ColectionData
{
    public class IntArrayTestt
    {
        [Fact]

        public void ReturnNullForNullAndEmptyForEmpty()
        {
            var input = new IntArray();
            input.Add(5);
            Assert.Equal(1, input.Count());
        }
    }
}
