using System;

namespace JsonValidation
{
    class ReadFromFile
    {
        static void Main()
        {
            string text = System.IO.File.ReadAllText(@"E:\Modulul 2\JsonValidation\JSONText.txt");
            var value = new Value(); 
            if (value.Match(text).RemainingText() == "")
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
