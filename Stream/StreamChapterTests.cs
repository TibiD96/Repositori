using Xunit;
using System.Security.Cryptography;


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
            var aes = Aes.Create();
            using MemoryStream stream = new();
            IStreamBuilder streamChapter = new StreamChapter(aes);
            var text = "test";
            var baseStream = streamChapter.BuildStreamWriter(stream);
            BuilderStreamChapter.StreamWriter(baseStream, text);
            stream.Seek(0, SeekOrigin.Begin);
            var decoded = new StreamChapter(aes).BuildStreamReader(stream);
            var result = BuilderStreamChapter.StreamReader(decoded);
            Assert.Equal(text, result);
        }

        [Fact]

        public void CheckIfCompressAndDecompressMethodesWorks()
        {
            var aes = Aes.Create();
            using MemoryStream stream = new();
            IStreamBuilder streamChapter = new StreamChapter(aes);
            var text = "test";
            var baseStream = streamChapter.BuildStreamWriter(stream, true);
            BuilderStreamChapter.StreamWriter(baseStream, text, true);
            stream.Seek(0, SeekOrigin.Begin);
            var decoded = new StreamChapter(aes).BuildStreamReader(stream, true);
            var result = BuilderStreamChapter.StreamReader(decoded, true);
            Assert.Equal(text, result);
        }


        [Fact]

        public void CheckIfCryptAndDecryptMethodesWorksWithNoZip()
        {
            var aes = Aes.Create();
            using MemoryStream stream = new();
            IStreamBuilder streamChapter = new StreamChapter(aes);
            var text = "test";
            var baseStream = streamChapter.BuildStreamWriter(stream, false, true);
            BuilderStreamChapter.StreamWriter(baseStream, text, false, true);
            stream.Seek(0, SeekOrigin.Begin);
            var decoded = new StreamChapter(aes).BuildStreamReader(stream, false, true);
            var result = BuilderStreamChapter.StreamReader(decoded, false, true);
            Assert.Equal(text, result);
        }

        [Fact]

        public void CheckIfCryptAndDecryptMethodesWorksWithZip()
        {
            var aes = Aes.Create();
            using MemoryStream stream = new();
            IStreamBuilder streamChapter = new StreamChapter(aes);
            var text = "test";
            var baseStream = streamChapter.BuildStreamWriter(stream, true, true);
            BuilderStreamChapter.StreamWriter(baseStream, text, true, true);
            stream.Seek(0, SeekOrigin.Begin);
            var decoded = new StreamChapter(aes).BuildStreamReader(stream, true, true);
            var result = BuilderStreamChapter.StreamReader(decoded, true, true);
            Assert.Equal(text, result);
        }
    }
}
