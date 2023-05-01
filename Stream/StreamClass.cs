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

        public void Writer(string text)
        {
            using (StreamWriter writer = new StreamWriter(Memory, leaveOpen: true))
            {
                writer.Write(text);
                writer.Flush();
            }
        }

        public string Reader()
        {
            Memory.Seek(0, SeekOrigin.Begin);
            using(StreamReader reader = new StreamReader(Memory))
            {
                return reader.ReadToEnd();
            }
        }

        public void CompressStream()
        {
            using var compressor = new GZipStream(Memory, CompressionMode.Compress);
        }

        public void DecompressStream()
        {
            if (Memory.CanRead || !Memory.CanSeek)
            {
                Memory.Seek(0, SeekOrigin.Begin);
                using var decompressor = new GZipStream(Memory, CompressionMode.Decompress);
                var output = new MemoryStream();
                decompressor.CopyTo(output);
                Memory = output;
                decompressor.Dispose();

            }
            else
            {
                throw new ArgumentException("Input is not compressed");
            }
        }
       
    }
}