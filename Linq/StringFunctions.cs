using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Linq
{
    public class StringFunctions
    {
        public static (int, int) CountVowelsAndConsons(string input)
        {
            int countVowels = 0;
            int countConsons = 0;

            return input.Where(element => char.IsLetter(element))
                        .Aggregate((countVowels, countConsons), (_, element) => Vowels(element) ? (++countVowels, countConsons) : (countVowels, ++countConsons));
        }

        public static char FirstUniqElement(string input)
        {
            return input.GroupBy(count => count).First(character => character.Count() == 1).Key;
        }

        public static int StringToInteger(string input)
        {
            if (input.Any(element => !char.IsDigit(element)))
            {
                throw new InvalidOperationException("Input is not correct format");
            }

            return input.Aggregate(0, (current, element) => current * 10 + (element - '0'));
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
            return Enumerable.Range(0, input.Length).SelectMany(startIndex => Enumerable.Range(1, input.Length - startIndex)
                                                    .Select(length => input.Substring(startIndex, length)))
                                                    .Where(substring => substring.SequenceEqual(substring.Reverse()))
                                                    .ToList();
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
