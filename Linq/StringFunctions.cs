using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class StringFunctions
    {
        public static (int, int) CountVowelsAndConsons(string input)
        {
            int countVowels = 0;
            int countConsons = 0;
            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    if (Vowels(c))
                    {
                        countVowels++;
                    }
                    else
                    {
                        countConsons++;
                    }
                }
            }

            return (countVowels, countConsons);
        }

        public static char FirstUniqElement(string input)
        {
            var numLetter = input.GroupBy(c => c).ToDictionary(l => l.Key, l => l.Count());

            foreach (char c in input)
            {
                if (numLetter[c] == 1)
                {
                    return c;
                }
            }

            throw new InvalidOperationException("No unique element found.");
        }

        public static int StringToInteger(string input)
        {
            if (input.All(c => char.IsDigit(c)))
            {
                return int.Parse(input);
            }

            throw new InvalidOperationException("Input is not correct format");
        }

        private static bool Vowels(char character)
        {
            foreach (char c in new[] { 'a', 'e', 'i', 'o', 'u' })
            {
                if (char.ToLower(character) == c)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
