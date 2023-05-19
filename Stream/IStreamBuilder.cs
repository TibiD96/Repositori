using System.Security.Cryptography;


namespace StreamClassProgram
{
    public interface IStreamBuilder
    {
        Stream BuildStreamWriter(Stream stream, bool gzip = false, bool crypt = false);
        Stream BuildStreamReader(Stream stream, bool gzip = false, bool crypt = false);
    }
    public class BuilderStreamChapter
    {
        public static void StreamWriter(Stream stream, string text, bool gzip = false, bool crypt = false)
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();

            if (stream is CryptoStream cryptoStream)
            {
                cryptoStream.FlushFinalBlock();
            }
        }

        public static string StreamReader(Stream stream, bool gzip = false, bool crypt = false)
        {
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();

        }
    }
}
