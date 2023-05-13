using System.IO.Compression;
using System.Security.Cryptography;


namespace StreamClassProgram
{
    public class StreamBuilder
    {
        public static Aes aes { get; } = Aes.Create();

        public static Stream GzipedStream(Stream stream)
        {
            return new GZipStream(stream, CompressionMode.Compress);
        }

        public static Stream UnzipedStream(Stream stream)
        {
            return new GZipStream(stream, CompressionMode.Decompress);
        }

        public static Stream CryptStream(Stream stream)
        {
            ICryptoTransform transform;
            transform = aes.CreateEncryptor();
            return new CryptoStream(stream, transform, CryptoStreamMode.Write);
        }

        public static Stream DecryptStream(Stream stream)
        {
            ICryptoTransform transform;
            transform = aes.CreateDecryptor();
            return new CryptoStream(stream, transform, CryptoStreamMode.Read);
        }

    }
}
