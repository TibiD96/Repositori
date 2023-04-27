using System;
using System.IO;

namespace Stream
{
    public class StreamClass
    {
        private readonly StreamClass file;
        public string Name { get; set; }
        
        public StreamClass(string name)
        {
            Name = name;
        }

        public void Writer(StreamClass file, string text)
        {
            using (StreamWriter writer = new StreamWriter(file.Name))
            {
                writer.Write(text);
            }
        }
       
    }
}