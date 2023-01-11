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

            if (input.Contains('\\'))
            {
                return CheckEscapedCharacters(input);
            }

            if (ContainsLongUnicodeCharacter(input))
            {
                return true;
            }

            return true;
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
            const string escapedCharactersToCheck = "\"/bfnrtu";
            int countCorrectEscapedCharacter = 0;
            int j = 0;
            while (j < input.Length)
            {
                if (input[j] == '\\')
                {
                    int nextElementAfterBackSlash = j + 1;
                    if (input[nextElementAfterBackSlash] == '\\')
                    {
                        j++;
                        countCorrectEscapedCharacter++;
                    }
                    else
                    {
                        CheckForRightCharacters(input, ref countCorrectEscapedCharacter, escapedCharactersToCheck, nextElementAfterBackSlash);
                    }

                    if (countCorrectEscapedCharacter == 0)
                    {
                        return false;
                    }
                }

                j++;
                countCorrectEscapedCharacter = 0;
            }

            return true;
        }

        static bool ContainsLongUnicodeCharacter(string input)
        {
            const int AsciiTableElements = 127;
            foreach (char element in input)
            {
                if (element > AsciiTableElements)
                {
                    return true;
                }
            }

            return false;
        }

        static void CheckForRightCharacters(string input, ref int countCorrectEscapedCharacter, string escapedCharactersToCheck, int nextElementAfterBackSlash)
        {
            for (int x = 0; x < escapedCharactersToCheck.Length; x++)
            {
                if (input[nextElementAfterBackSlash] == escapedCharactersToCheck[x] && x != escapedCharactersToCheck.Length - 1 && nextElementAfterBackSlash != input.Length - 1)
                {
                    countCorrectEscapedCharacter++;
                }

                if (input[nextElementAfterBackSlash] == escapedCharactersToCheck[x] && x == escapedCharactersToCheck.Length - 1)
                {
                    CheckForEscapedUnicode(input, nextElementAfterBackSlash, ref countCorrectEscapedCharacter);
                }
            }
        }

        static void CheckForEscapedUnicode(string input, int nextElementAfterBack, ref int countCorrectEscapedCharacter)
        {
            const int minHexNumbers = 4;
            const int maxHexNumbers = 6;
            int hexCounter = 0;
            char[] hexcharacter = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D', 'E', 'F' };
            for (int i = nextElementAfterBack + 1; i < input.Length; i++)
            {
                if (Array.IndexOf(hexcharacter, input[i]) > -1)
                {
                    hexCounter++;
                }

                if (hexCounter >= minHexNumbers && hexCounter <= maxHexNumbers)
                {
                    countCorrectEscapedCharacter++;
                }
            }
        }
    }
}