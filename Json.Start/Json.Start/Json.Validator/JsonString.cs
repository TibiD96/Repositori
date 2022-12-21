using System;
using System.Linq;

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

            if (input == "\"\"")
            {
                return true;
            }

            if (!DontContainControlCharacter(input))
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

        static bool DontContainControlCharacter(string input)
        {
            const int asciiVerification = 32;

            foreach (char character in input)
            {
                if (character < asciiVerification)
                {
                    return false;
                }
            }

            return true;
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
            const int MaximAnsi = 255;
            return input.Any(element => element > MaximAnsi);
        }
    }
}
