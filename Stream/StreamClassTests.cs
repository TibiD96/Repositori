using Xunit;

namespace StreamClassProgram
{
    public class StreamClassTests
    {
        [Fact]

        public void CheckIfConstructorWorks()
        {
            Stream file = new Stream();
            Assert.NotNull(file.Memory);

        }

        [Fact]

        public void CheckIfWriterAndReaderMethodeWorks()
        {
            Stream stream = new Stream();
            stream.Writer(stream, "test");
            string textToCheck = stream.Reader(stream);
            Assert.Equal("test", textToCheck);
        }
    }
}
