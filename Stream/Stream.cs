using System;
using System.IO;

namespace Stream
{
    public class Stream
    {
        public string Name { get; set; }
        public int Length { get; set; }
        
        public Stream(string name, int length)
        {
            Name = name;
            Length = length;
        }
       
    }
}