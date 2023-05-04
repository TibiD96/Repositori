using Xunit;

namespace StreamClassProgram
{
    public class StreamChapterTests
    {
        [Fact]

        public void CheckIfConstructorWorks()
        {
            var file = new MemoryStream();
            Assert.NotNull(file);
        }

        [Fact]

        public void CheckIfWriterAndReaderMethodeWorks()
        {
            var stream = new MemoryStream();
            StreamChapter.Writer(stream, "test");
            string textToCheck = StreamChapter.Reader(stream);
            Assert.Equal("test", textToCheck);
        }

        [Fact]

        public void CheckIfCompressAndDecompressMethodesWorks()
        {
            var stream = new MemoryStream();
            StreamChapter.Writer(stream, "test", true);
            string result = StreamChapter.Reader(stream, true);
            Assert.Equal("test", result);
        }

        [Fact]

        public void CheckIfCryptAndDecryptMethodesWorksWithNoZip()
        {
            var stream = new MemoryStream();
            StreamChapter.Writer(stream, "test", false, true);
            stream.Seek(0, SeekOrigin.Begin);
            string result = StreamChapter.Reader(stream, false, true);
            Assert.Equal("test", result);
        }

        [Fact]

        public void CheckIfCryptAndDecryptMethodesWorksWithZip()
        {
            var stream = new MemoryStream();
            StreamChapter.Writer(stream, "test", true, true);
            string result = StreamChapter.Reader(stream, true, true);
            Assert.Equal("test", result);
        }

        [Fact]

        public void CheckIfCryptMethodesWorks()
        {
            var stream = new MemoryStream();
            StreamChapter.Writer(stream, "test", false, false);
            string result = StreamChapter.Reader(stream, false, false);
            Assert.Equal("test", result);
        }
    }
}
