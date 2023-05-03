using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace StreamClassProgram
{
    public class StreamChapter
    {
        public MemoryStream Memory { get; private set; }
        
        public StreamChapter()
        {
            Memory = new MemoryStream();
        }

        public static void Writer(Stream stream, string text, bool gzip = false, bool crypt = false)
        {
            StreamWriter writer;
            Stream support = stream;
            if (gzip)
            {
               support = new GZipStream(stream, CompressionMode.Compress, true);
            }

            if (crypt)
            {
                using Aes aes = Aes.Create();
                aes.GenerateKey();
                aes.GenerateIV();

                ICryptoTransform encryptor = aes.CreateEncryptor();

                support = new CryptoStream(support, encryptor, CryptoStreamMode.Write, true);
            }

            writer = new StreamWriter(support, leaveOpen: true);
            writer.Write(text);
            writer.Flush();

        }

        public static string Reader(Stream stream, bool gzip = false, bool crypt = false)
        {
            stream.Seek(0, SeekOrigin.Begin);
            Stream support = stream;

            if (gzip)
            {
                support = new GZipStream(stream, CompressionMode.Decompress, true);
            }
            
            if (crypt)
            {
                using Aes aes = Aes.Create();
                byte[] iv = new byte[aes.IV.Length];
                stream.Read(iv, 0, iv.Length);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor();

                support = new CryptoStream(support, decryptor, CryptoStreamMode.Read, true);
            }
            using var reader = new StreamReader(support);
            return reader.ReadToEnd();
            
        }
    }
}