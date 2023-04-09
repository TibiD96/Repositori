using Xunit;

namespace CollectionData
{
    public class ListTests
    {
        [Fact]

        public void CheckIfConstructorWorks()
        {
            var list = new List<int>();
            Assert.Equal(0, list.Count);
            Assert.Equal(0, list[0]);
            Assert.Equal(0, list[1]);
            Assert.Equal(0, list[2]);

        }
    }
}