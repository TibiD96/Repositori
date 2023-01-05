using System;
using System.Text.RegularExpressions;

namespace Json
{
    public static class JsonString
    {
        public static bool IsJsonString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            if (!input.StartsWith("\"") || !input.EndsWith("\""))
            {
                return false;
            }

            if (ContainControlCharacter(input))
            {
                return false;
            }

            if (input.Contains('\\'))
            {
                return CheckEscapedCharacters(input);
            }

            if (ContainsLongUnicodeCharacter(input))
            {
                return true;
            }

            return input.StartsWith('"') && input.EndsWith('"');
        }

        static bool ContainControlCharacter(string input)
        {
            const int asciiVerification = 32;

            foreach (char character in input)
            {
                if (character < asciiVerification)
                {
                    return true;
                }
            }

            return false;
        }

        static bool CheckEscapedCharacters(string input)
        {
            string[] escapedCharactersToCheck = { @"\""", @"\\", @"\/", @"\b", @"\f", @"\n", @"\r", @"\t", @"\u" };
            char[] hexcharacter = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D', 'E', 'F' };
            int indexOfBackslash = input.IndexOf('\\');
            const int hexNumbers = 4;
            int hexCounter = 0;
            int nextElementAfterBackSlach = indexOfBackslash + 1;
            for (int j = 0; j < escapedCharactersToCheck.Length - 1; j++)
            {
                if (input.Contains(escapedCharactersToCheck[j]) && nextElementAfterBackSlach != input.Length - 1)
                {
                    return true;
                }
            }

            nextElementAfterBackSlach++;
            for (int i = nextElementAfterBackSlach; i < input.Length; i++)
            {
                if (Array.IndexOf(hexcharacter, input[i]) > -1)
                {
                    hexCounter++;
                }

                if (hexCounter == hexNumbers)
                {
                    return true;
                }
            }

            return false;
        }

        static bool ContainsLongUnicodeCharacter(string input)
        {
            return Regex.IsMatch(input, @"[\u2600-\u26FF]");
        }
    }
}