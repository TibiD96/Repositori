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
            stream.Writer("test");
            string textToCheck = stream.Reader();
            Assert.Equal("test", textToCheck);
        }

        [Fact]

        public void CheckIfCompressAndDecompressMethodesWorks()
        {
            var stream = new Stream();
            stream.Writer("test");
            stream.CompressStream();
            stream.DecompressStream();
            string result = stream.Reader();

            Assert.Equal("test", result);
        }
    }
}
