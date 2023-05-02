using System;
using System.IO;
using System.IO.Compression;

namespace StreamClassProgram
{
    public class Stream
    {
        public MemoryStream Memory { get; private set; }
        
        public Stream()
        {
            Memory = new MemoryStream();
        }

        public void Writer(string text, bool gzip = false)
        {
            if (gzip)
            {
                using var compressor = new GZipStream(Memory, CompressionMode.Compress, true);
                using var writer = new StreamWriter(compressor);
                writer.Write(text);
            }
            else
            {
                using var writer = new StreamWriter(Memory, leaveOpen: true);
                writer.Write(text);
            }
        }

        public string Reader(bool gzip = false)
        {
            Memory.Seek(0, SeekOrigin.Begin);

            if (gzip)
            {
                using var decompressor = new GZipStream(Memory, CompressionMode.Decompress);
                using var reader = new StreamReader(decompressor);
                return reader.ReadToEnd();
            }
            else
            {
                using var reader = new StreamReader(Memory);
                return reader.ReadToEnd();
            }
        }
    }
}