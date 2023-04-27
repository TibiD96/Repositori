using System;
using System.IO;

namespace StreamClassProgram
{
    public class Stream
    {
        public MemoryStream Memory { get; private set; }
        
        public Stream()
        {
            Memory = new MemoryStream();
        }

        public void Writer(Stream input, string text)
        {
            using (StreamWriter writer = new StreamWriter(input.Memory, leaveOpen: true))
            {
                writer.Write(text);
                writer.Flush();
            }
        }

        public string Reader(Stream input)
        {
            Memory.Seek(0, SeekOrigin.Begin);
            using(StreamReader reader = new StreamReader(input.Memory))
            {
                return reader.ReadToEnd();
            }
        }
       
    }
}