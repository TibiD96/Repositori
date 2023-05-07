﻿using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace StreamClassProgram
{
    public class StreamChapter
    {
        public MemoryStream Memory { get; private set; }
        public static byte[] Key = new byte[16];
        public static byte[] Iv = new byte[16];

        private static byte[] cryptedText;

        public StreamChapter()
        {
            Memory = new MemoryStream();

        }

        public static void Writer(Stream stream, string text, bool gzip = false, bool crypt = false)
        {
            Stream support = stream;
            StreamWriter writer;
            MemoryStream memorySuport = new MemoryStream();

            if (gzip)
            {
               support = new GZipStream(stream, CompressionMode.Compress, true);
            }

            if (crypt)
            {
                using (Aes aes = Aes.Create())
                {
                    ICryptoTransform encryptor = aes.CreateEncryptor(Key, Iv);
                    using (support = new CryptoStream(memorySuport, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(support))
                        {
                            streamWriter.Write(text);
                        }

                        cryptedText = memorySuport.ToArray();
                    }
                }

                return;
            }

            writer = new StreamWriter(support);       
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
                string simpletext = String.Empty;
                using (Aes aes = Aes.Create())
                {
                    ICryptoTransform decryptor = aes.CreateDecryptor(Key, Iv);
                    using MemoryStream memoryStream = new MemoryStream(cryptedText);
                    using (stream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using StreamReader streamReader = new StreamReader(stream);
                        simpletext = streamReader.ReadToEnd();
                    }
                }

                return simpletext;
            }

            StreamReader reader = new StreamReader(support, true);
            return reader.ReadToEnd();
        }
    }
}
