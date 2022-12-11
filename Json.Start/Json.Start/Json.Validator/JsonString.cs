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

            if (input.StartsWith('"') && input.EndsWith('"'))
            {
                return ValidateString(input);
            }

            return false;
        }

        static bool ValidateString(string input)
        {
            if (!DontContainControlCharacter(input))
            {
                return false;
            }

            if (CheckEscapedCharacters(input))
            {
                return true;
            }

            if (CheckForValidEscapedUnicodeCharactersCheckToNotFinishWithAnUnifinishedHexNumber(input))
            {
                return true;
            }

            if (!CheckForValidEscapedUnicodeCharactersCheckToNotFinishWithAnUnifinishedHexNumber(input) && input.Contains("\\"))
            {
                return false;
            }

            return input.StartsWith('"') && input.EndsWith('"');
        }

        static bool DontContainControlCharacter(string input)
        {
            string[] controlCharacters = { "\b", "\f", "\n", "\r", "\t" };
            for (int j = 0; j < controlCharacters.Length; j++)
                {
                    if (input.Contains(controlCharacters[j]))
                    {
                        return false;
                    }
            }

            return true;
        }

        static bool CheckEscapedCharacters(string input)
        {
            char[] escapedCharactersToCheck = { '\"', '\\', '/', 'b', 'f', 'n', 'r', 't' };
            int i = input.IndexOf('\\');
            int nextElement = i + 1;
            for (int j = 0; j < escapedCharactersToCheck.Length; j++)
            {
                        if (input[nextElement] == escapedCharactersToCheck[j] && nextElement != input.Length - 1)
                        {
                            return true;
                        }
            }

            return false;
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
