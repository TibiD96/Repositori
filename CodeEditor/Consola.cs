using System;
using System.Collections.Generic;

namespace CodeEditor
{
    public class Consola
    {
        public static void Menu()
        {
            Console.WriteLine("1. Show from file");
        }

        public static void ShowContentOfFile()
        {
            foreach (string line in File.ReadAllLines("E:\\JUniorMind\\Modulul_2\\Repositori\\CodeEditor\\File.txt"))
            {
                Console.WriteLine(line);
            }
        }
    }
}
