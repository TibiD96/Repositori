using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace StreamClassProgram
{
    public class StreamChapter
    {
        public static Aes aes { get; } = Aes.Create();

        public static void Writer(Stream stream, string text, bool gzip = false, bool crypt = false)
        {
            Stream support = stream;
            StreamWriter writer;

            if (gzip)
            {
                support = new GZipStream(support, CompressionMode.Compress, true);
            }

            if (crypt)
            {
                ICryptoTransform encryptor = aes.CreateEncryptor();
                support = new CryptoStream(support, encryptor, CryptoStreamMode.Write, true);
            }

            writer = new StreamWriter(support);
            writer.Write(text);
            writer.Flush();

            if (support is CryptoStream cryptoStream)
            {
                cryptoStream.FlushFinalBlock();
            }
        }

        public static string Reader(Stream stream, bool gzip = false, bool crypt = false)
        {
            stream.Seek(0, SeekOrigin.Begin);
            Stream support = stream;

            if (gzip)
            {
                support = new GZipStream(support, CompressionMode.Decompress, true);
            }

            if (crypt)
            {
                ICryptoTransform decryptor = aes.CreateDecryptor();
                support = new CryptoStream(support, decryptor, CryptoStreamMode.Read);
            }

            using StreamReader reader = new StreamReader(support);
            return reader.ReadToEnd();
        }
    }
}