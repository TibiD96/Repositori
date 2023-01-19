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
            for (int indexOfCurrentCaracter = 0; indexOfCurrentCaracter < input.Length; indexOfCurrentCaracter++)
            {
                if (input[indexOfCurrentCaracter] == '\\')
                {
                    int nextElementAfterBackSlash = indexOfCurrentCaracter + 1;

                    if (nextElementAfterBackSlash == input.Length - 1)
                    {
                        return false;
                    }

                    if (!CheckForRightEscapedCharacters(input, nextElementAfterBackSlash, ref indexOfCurrentCaracter))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        static bool CheckForRightEscapedCharacters(string input, int nextElementAfterBackSlash, ref int indexOfNextCharacter)
        {
            const string correctEscapedCharacters = "\"/bfnrtu";
            if (input[nextElementAfterBackSlash] == '\\')
            {
                indexOfNextCharacter++;
                return true;
            }

            if (correctEscapedCharacters.Contains(input[nextElementAfterBackSlash]) && input[nextElementAfterBackSlash] != correctEscapedCharacters[correctEscapedCharacters.Length - 1])
            {
                    return true;
            }

            return correctEscapedCharacters.Contains(input[nextElementAfterBackSlash]) && CheckForEscapedUnicode(input, nextElementAfterBackSlash);
        }

        static bool CheckForEscapedUnicode(string input, int index)
        {
            const int hexSequenceLength = 4;
            if (input.Length - index < hexSequenceLength)
            {
                return false;
            }

            for (int i = 1; i <= hexSequenceLength; i++)
            {
                if (!IsHexChar(input[i + index]))
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsHexChar(char input)
        {
            input = char.ToLower(input);
            return (input >= 'a' && input <= 'f')
                || char.IsDigit(input);
        }
    }
}