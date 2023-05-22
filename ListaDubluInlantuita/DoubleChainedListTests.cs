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
    }
}
