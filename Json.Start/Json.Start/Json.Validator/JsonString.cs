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

            return input.StartsWith('"') && input.EndsWith('"');
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

            if (!DontEndWithReverseSolidus(input))
            {
                return false;
            }

            return !input.Contains("\\") || CheckEscapedCharacters(input);
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

        static bool DontEndWithReverseSolidus(string input)
        {
            int i = input.Length - 1;
            return input[i - 1] != '\\';
        }

        static bool CheckEscapedCharacters(string input)
        {
            string[] escapedCharacters = { @"\""", @"\\", @"\/", @"\b", @"\f", @"\n", @"\r", @"\t" };
            for (int i = 0; i < escapedCharacters.Length; i++)
            {
                if (input.Contains(escapedCharacters[i]))
                {
                    return true;
                }
            }

            return CheckForValidEscapedUnicodeCharactersCheckToNotFinishWithAnUnifinishedHexNumber(input);
        }

        static bool CheckForValidEscapedUnicodeCharactersCheckToNotFinishWithAnUnifinishedHexNumber(string input)
        {
            int countHex = 0;
            int lengthOfStringCountFrom0 = input.Length - 1;
            const int numberOfAValidHex = 4;
            for (int j = 0; j < input.Length; j++)
            {
                lengthOfStringCountFrom0--;
                if (input[j] == 'u' && j < lengthOfStringCountFrom0)
                {
                    for (int i = j + 1; input[i] != ' '; i++)
                    {
                        countHex++;
                    }

                    break;
                }
                else if (input[j] == 'u' && j == lengthOfStringCountFrom0 - 1)
                {
                    return false;
                }
            }

            return countHex >= numberOfAValidHex;
        }
    }
}
