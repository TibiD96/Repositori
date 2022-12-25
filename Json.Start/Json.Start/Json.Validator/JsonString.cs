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

            if (ContainsLongUnicodeCharacter(input))
            {
                return true;
            }

            if (input.Contains('\\'))
            {
                return CheckEscapedCharacters(input);
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
            int indexOfBackslash = input.IndexOf('\\');
            int nextElementAfterBackSlach = indexOfBackslash + 1;
            int lengthOfUnicode = 0;
            const int corectLengthOfUnicode = 5;
            for (int j = 0; j < escapedCharactersToCheck.Length; j++)
            {
               if (input.Contains(escapedCharactersToCheck[j]) && nextElementAfterBackSlach != input.Length - 1 && j < escapedCharactersToCheck.Length - 1)
               {
                 return true;
               }

               while (nextElementAfterBackSlach < input.Length && input.Contains(escapedCharactersToCheck[j]) && j == escapedCharactersToCheck.Length - 1)
                {
                    if (input[nextElementAfterBackSlach] == ' ' && lengthOfUnicode == corectLengthOfUnicode)
                    {
                            return true;
                    }

                    nextElementAfterBackSlach++;
                    lengthOfUnicode++;
                }
            }

            return false;
        }

        static bool ContainsLongUnicodeCharacter(string input)
        {
            return Regex.IsMatch(input, @"\p{IsMiscellaneousSymbols}");
        }
    }
}
