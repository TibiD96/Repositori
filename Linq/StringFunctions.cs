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
