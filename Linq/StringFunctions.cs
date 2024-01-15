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
            var groupOfChars = input.GroupBy(element => element);
            var maxAppearances = groupOfChars.Max(element => element.Count());
            return groupOfChars.Where(element => element.Count() == maxAppearances).Select(element => element.Key).ToArray();
        }

        public static IEnumerable<string> DivideStringInPalindroms(string input)
        {
            return Enumerable.Range(0, input.Length).SelectMany(startIndex => Enumerable.Range(1, input.Length - startIndex)
                                                    .Select(length => input.Substring(startIndex, length)))
                                                    .Where(substring => substring.SequenceEqual(substring.Reverse()));
        }

        private static bool Vowels(char character)
        {
            char[] vowels = new[] { 'a', 'e', 'i', 'o', 'u' };
            return vowels.Any(element => element == char.ToLower(character));
        }
    }
}
