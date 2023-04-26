using Xunit;

namespace Stream
{
    public class StreamTests
    {
        [Fact]

        public void CheckIfConstructorWorks()
        {
            Stream file = new Stream("test stream", 10);
            Assert.Equal("test stream", file.Name);
            Assert.Equal(10, file.Length);

        }
    }
}
