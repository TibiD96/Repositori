using System;

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

            return !input.Contains('\\') || CheckEscapedCharacters(input);
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
            int j = 0;
            while (j < input.Length)
            {
                if (input[j] == '\\')
                {
                    int nextElementAfterBackSlash = j + 1;
                    if (nextElementAfterBackSlash == input.Length - 1)
                    {
                        return false;
                    }

                    if (input[nextElementAfterBackSlash] == '\\')
                    {
                        j++;
                    }
                    else if (!CheckForRightEscapedCharacters(input, nextElementAfterBackSlash))
                    {
                        return false;
                    }

                    j++;
                }
                else
                {
                    j++;
                }
            }

            return true;
        }

        static bool CheckForRightEscapedCharacters(string input, int nextElementAfterBackSlash)
        {
            const string correctEscapedCharacters = "\"/bfnrtu";
            if (correctEscapedCharacters.Contains(input[nextElementAfterBackSlash]) && input[nextElementAfterBackSlash] != correctEscapedCharacters[correctEscapedCharacters.Length - 1])
            {
                    return true;
            }

            return correctEscapedCharacters.Contains(input[nextElementAfterBackSlash]) && CheckForEscapedUnicode(input, nextElementAfterBackSlash);
        }

        static bool CheckForEscapedUnicode(string input, int nextElementAfterBackSlash)
        {
            int hexCounter = 0;
            for (int i = nextElementAfterBackSlash + 1; i < input.Length && input[i] != ' '; i++)
            {
                if (CheckToBeACorrectUnicode(input, i, ref hexCounter))
                {
                    return true;
                }
            }

            return false;
        }

        static bool CheckToBeACorrectUnicode(string input, int i, ref int hexCounter)
        {
            const int minHexNumbers = 4;
            const int maxHexNumbers = 6;
            if ((input[i] >= 'a' && input[i] <= 'f') || (input[i] >= 'A' && input[i] <= 'F'))
            {
                hexCounter++;
            }

            if (input[i] >= '0' && input[i] <= '9')
            {
                hexCounter++;
            }

            return hexCounter >= minHexNumbers && hexCounter <= maxHexNumbers;
        }
    }
}