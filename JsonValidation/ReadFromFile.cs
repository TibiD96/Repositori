using System;

namespace JsonValidation
{
    class ReadFromFile
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Add a input");
                return;
            }

            string text = System.IO.File.ReadAllText(args[0]);
            var value = new Value(); 
            if (args.Length > 0 && value.Match(text).RemainingText() == "")
            {
                Console.WriteLine("Text from file is Json Valid");
            }
            else
            {
                Console.WriteLine("Text from file is not Json Valid");
            }
        }
    }
}
