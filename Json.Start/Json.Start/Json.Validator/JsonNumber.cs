using System;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            if (input.Contains('e') || input.Contains('E'))
            {
                return CheckToBeACorrectStringWithExponent(input);
            }

            if (input.Contains('.'))
            {
                return CheckToBeACorrectFractionalNumberInStringForm(input);
            }

            if (input.StartsWith('0') && input.Length > 1)
            {
                return false;
            }

            if (DoesNotContainLetters(input))
            {
                return false;
            }

            return true;
        }

        public static bool CheckToBeACorrectFractionalNumberInStringForm(string input)
        {
            int dotCounter = 0;
            if (input.EndsWith('.'))
            {
                return false;
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '.')
                {
                    dotCounter++;
                }

                if (char.IsLetter(input, i))
                {
                    return false;
                }

                if (dotCounter > 1)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckToBeACorrectStringWithExponent(string input)
        {
            if (!char.IsDigit(input, input.Length - 1))
            {
                return false;
            }

            if (input.StartsWith('e') || input.StartsWith('E'))
            {
                return false;
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == 'e' || input[i] == 'E')
                {
                    int nextElementAfterExponent = i + 1;

                    return CheckNextCharactersOfTheStringAfterExponent(input, nextElementAfterExponent);
                }
            }

            return true;
        }

        static bool CheckNextCharactersOfTheStringAfterExponent(string input, int nextElementAfterExponent)
        {
            for (int j = nextElementAfterExponent; j < input.Length; j++)
            {
                if (input[j] == '.')
                {
                    return false;
                }

                if (char.IsLetter(input, j))
                {
                    return false;
                }
            }

            return true;
        }

        static bool DoesNotContainLetters(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
              if (char.IsLetter(input, i))
              {
                    return true;
              }
            }

            return false;
        }
    }
}
