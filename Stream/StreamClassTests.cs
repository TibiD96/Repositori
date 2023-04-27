using Xunit;

namespace Stream
{
    public class StreamClassTests
    {
        [Fact]

        public void CheckIfConstructorWorks()
        {
            StreamClass file = new StreamClass("test stream");
            Assert.Equal("test stream", file.Name);

        }

        [Fact]

        public void CheckIfWriterMethodeWorks()
        {
            StreamClass file = new StreamClass("test stream");
            file.Writer(file, "test");
            var textFromFile = File.ReadAllText(file.Name);
            Assert.Equal("test", textFromFile);

        }
    }
}
