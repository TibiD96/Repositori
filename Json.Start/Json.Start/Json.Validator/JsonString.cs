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

            for (int x = 0; x < correctEscapedCharacters.Length; x++)
            {
                if (input[nextElementAfterBackSlash] == correctEscapedCharacters[x] && x != correctEscapedCharacters.Length - 1 && nextElementAfterBackSlash != input.Length - 1)
                {
                    return true;
                }

                if (input[nextElementAfterBackSlash] == correctEscapedCharacters[x] && x == correctEscapedCharacters.Length - 1)
                {
                    return CheckForEscapedUnicode(input, nextElementAfterBackSlash);
                }
            }

            return false;
        }

        static bool CheckForEscapedUnicode(string input, int nextElementAfterBackSlash)
        {
            const int minHexNumbers = 4;
            const int maxHexNumbers = 6;
            int hexCounter = 0;
            const string hexCharacters = "0123456789abcdefABCDEF";
            for (int i = nextElementAfterBackSlash + 1; i < input.Length; i++)
            {
                for (int j = 0; j < hexCharacters.Length; j++)
                {
                    if (input[i] == hexCharacters[j])
                    {
                        hexCounter++;
                    }
                }

                if (hexCounter >= minHexNumbers && hexCounter <= maxHexNumbers)
                {
                    return true;
                }
            }

            return false;
        }
    }
}