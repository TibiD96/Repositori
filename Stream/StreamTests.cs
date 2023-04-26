using Xunit;

namespace Stream
{
    public class StreamTests
    {
        [Fact]

        public void CheckIfConstructorWorks()
        {
            Stream file = new Stream("test stream");
            Assert.Equal("test stream", file.Name);

        }

        [Fact]

        public void CheckIfWriterMethodeWorks()
        {
            Stream file = new Stream("test stream");
            file.Writer(file, "test");
            var textFromFile = File.ReadAllText(file.Name);
            Assert.Equal("test", textFromFile);

        }
    }
}
