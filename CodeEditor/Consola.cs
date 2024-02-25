using System;
using System.Collections.Generic;
using System.IO;

namespace CodeEditor
{
    public class Consola
    {
        public static void Menu()
        {
            Console.WriteLine("0. Exit application");
            Console.WriteLine("1. Show from file");
            Console.WriteLine("2. Show Menu");
        }

        public static void ShowContentOfFile(string fullPath, int startingLine = 0, int startingColumn = 0)
        {
            const int visibleAreaWidth = 119;
            const int visibleAreaHight = 30;
            string line;
            int currentEndColumn;
            int currentStartColumn;
            Console.Clear();
            string[] lines = File.ReadAllLines(fullPath);

            for (int i = startingLine; i < Math.Min(lines.Length, startingLine + visibleAreaHight); i++)
            {
                line = lines[i];
                currentStartColumn = Math.Max(0, Math.Min(startingColumn, line.Length));
                currentEndColumn = line.Length - currentStartColumn <= visibleAreaWidth ? line.Length - currentStartColumn : visibleAreaWidth;

                if (i < Math.Min(lines.Length, startingLine + visibleAreaHight) - 1)
                {
                    Console.WriteLine(line.Substring(currentStartColumn, currentEndColumn));
                }
                else
                {
                    Console.Write(line.Substring(currentStartColumn, currentEndColumn));
                }
            }
        }
    }
}
