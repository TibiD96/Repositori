using System;
using System.Collections.Generic;

namespace CodeEditor
{
    public class Consola
    {
        private static readonly string FilePath = Path.Combine("..", "..", "..", "File.txt");

        public static void Menu()
        {
            Console.WriteLine("0. Exit aplciation");
            Console.WriteLine("1. Show from file");
            Console.WriteLine("2. Show Menu");
        }

        public static void ShowContentOfFile()
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FilePath);
            foreach (string line in File.ReadAllLines(fullPath))
            {
                Console.WriteLine(line);
            }
        }
    }
}
