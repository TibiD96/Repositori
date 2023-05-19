using System.IO.Compression;
using System.Security.Cryptography;

namespace StreamClassProgram
{
    public class StreamChapter : IStreamBuilder
    {
        private readonly Aes aes;

        public StreamChapter(Aes aes)
        {
            this.aes = aes;
        }

        public Stream BuildStreamWriter(Stream stream, bool gzip = false, bool crypt = false)
        {
            if (gzip)
            {
               stream = new GZipStream(stream, CompressionMode.Compress);
            }

            if (crypt)
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                stream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
            }

            return stream;        
        }

        public Stream BuildStreamReader(Stream stream, bool gzip = false, bool crypt = false)
        {
            if (gzip)
            {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }

            if (crypt)
            {
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                stream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
            }

            return stream;
                
            
        }
    }
}