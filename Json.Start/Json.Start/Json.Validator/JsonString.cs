using System;
using System.Linq;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

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

            return StringChecker(input);
        }

        static bool StringChecker(string input)
        {
            if (input.StartsWith('"') && input.EndsWith('"'))
            {
                return QuotedString(input);
            }

            if (!input.StartsWith('"') || !input.EndsWith('"'))
            {
                return false;
            }

            return true;
        }

        static bool QuotedString(string input)
        {
            if (!DontContainControlCharacter(input))
            {
                return false;
            }

            if (CanContainUnicodeCharacters(input))
            {
                return true;
            }

            if (CheckEscapedCharacters(input))
            {
                return true;
            }

            return true;
        }

        static bool DontContainControlCharacter(string input)
        {
            char[] controlCharacters = { '\t', '\n', '\v', '\f', '\r' };
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < controlCharacters.Length; j++)
                {
                    if (input[i] == controlCharacters[j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        static bool CanContainUnicodeCharacters(string input)
        {
            const int asciiVerification = 127;
            foreach (char character in input)
            {
                if (character > asciiVerification)
                {
                    return true;
                }
            }

            return false;
        }

        static bool CheckEscapedCharacters(string input)
        {
            string[] escapedCharacters = { @"\""", @"\\", @"\/", @"\b", @"\f", @"\n", @"\r", @"\t", @"\u26Be" };
            for (int i = 0; i < escapedCharacters.Length; i++)
            {
                if (input.Contains(escapedCharacters[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
