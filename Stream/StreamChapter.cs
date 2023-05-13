using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace StreamClassProgram
{
    public class StreamChapter : StreamBuilder
    {
        public static void Writer(Stream stream, string text, bool gzip = false, bool crypt = false)
        {
            if (gzip)
            {
                GzipedStream(stream);
            }

            if (crypt)
            {
                CryptStream(stream);
            }

            StreamWriter writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            if (stream is CryptoStream cryptoStream)
            {
              cryptoStream.FlushFinalBlock();
            }         
        }

        public static string Reader(Stream stream, bool gzip = false, bool crypt = false)
        {
            if (gzip)
            {
                UnzipedStream(stream);
            }

            if (crypt)
            {
                DecryptStream(stream);
            }
            using StreamReader reader = new StreamReader(stream); 
            return reader.ReadToEnd();
                
            
        }
    }
}