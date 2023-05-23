using System.Collections.Generic;
using Xunit;

namespace ChainedList
{
    public class DoubleChainedListTests
    {
        [Fact]

        public void CheckIfConstructorWorksDefault()
        {
            var linkedList = new DoubleChainedList<int>();
            Assert.Empty(linkedList);
        }

        [Fact]

        public void CheckIfConstructorWorksWithInputArray()
        {
            int[] input = new[] {1, 2, 3};
            var linkedList = new DoubleChainedList<int>(input);
            Assert.Equal(1, linkedList.GetFirst.Value);
            Assert.Equal(3, linkedList.GetLast.Value);
            Assert.Equal(3, linkedList.Count);
        }
    }
}
