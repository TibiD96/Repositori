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
            var numLetter = input.GroupBy(count => count).ToDictionary(character => character.Key, character => character.Count());

            foreach (char character in input)
            {
                if (numLetter[character] == 1)
                {
                    return character;
                }
            }

            throw new InvalidOperationException("No unique element found.");
        }

        public static int StringToInteger(string input)
        {
            if (input.All(character => char.IsDigit(character)))
            {
                return int.Parse(input);
            }

            throw new InvalidOperationException("Input is not correct format");
        }

        public static char[] CharacterWithMostAppearances(string input)
        {
            int maxCount = 0;
            var numLetter = input.GroupBy(count => count).ToDictionary(character => character.Key, character => character.Count());
            foreach (char character in input)
            {
                if (numLetter[character] > maxCount)
                {
                    maxCount = numLetter[character];
                }
            }

            return numLetter.Where(count => count.Value == maxCount).Select(character => character.Key).ToArray();
        }

        public static List<string> DivideStringInPalindroms(string input)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = i + 1; j <= input.Length; j++)
                {
                    string temp = input[i..j];
                    var reversed = temp.ToCharArray();
                    Array.Reverse(reversed);
                    if (temp == new string(reversed))
                    {
                        list.Add(temp);
                    }
                }
            }

            return list;
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
