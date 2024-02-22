using System;
using System.Collections.Generic;

namespace CodeEditor
{
    class CodeEditor
    {
        static void Main()
        {
            Console.Write($"\x1b[8;{31};{120}t");
            Controller.RunMenu();
        }
    }
}
