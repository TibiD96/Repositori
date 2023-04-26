using System;
using System.IO;

namespace Stream
{
    public class Stream
    {
        private readonly Stream file;
        public string Name { get; set; }
        
        public Stream(string name)
        {
            Name = name;
        }

        public void Writer(Stream file, string text)
        {
            using (StreamWriter writer = new StreamWriter(file.Name))
            {
                writer.Write(text);
            }
        }
       
    }
}