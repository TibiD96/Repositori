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
                return StringQuoted(input);
            }

            if (!input.StartsWith('"') || !input.EndsWith('"'))
            {
                return false;
            }

            return true;
        }

        static bool StringQuoted(string input)
        {
            const int asciiVerification = 127;
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

            foreach (char character in input)
            {
                if (character > asciiVerification)
                {
                    return true;
                }
            }

            if (input.Contains("\"") || input.Contains("\\"))
            {
                return true;
            }

            return true;
        }
    }
}
