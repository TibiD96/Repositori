﻿using System;
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
                using (Rijndael rijAlg = Rijndael.Create())
                {
                    rijAlg.GenerateKey();
                    rijAlg.GenerateIV();

                    ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                    support = new CryptoStream(stream, encryptor, CryptoStreamMode.Write, true);   
                }
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
                using (Rijndael rijAlg = Rijndael.Create())
                {
                    rijAlg.GenerateKey();
                    rijAlg.GenerateIV();

                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                    support = new CryptoStream(stream, decryptor, CryptoStreamMode.Read, true);
                }
            }
            using var reader = new StreamReader(support);
            return reader.ReadToEnd();
            
        }
    }
}
