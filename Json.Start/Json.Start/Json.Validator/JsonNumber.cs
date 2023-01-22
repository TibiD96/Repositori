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
            if (input.EndsWith('.'))
            {
                return false;
            }

            int indexOfDot = input.IndexOf('.');

            for (int i = indexOfDot + 1; i < input.Length; i++)
            {
                if (!char.IsDigit(input, i))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckToBeACorrectStringWithExponent(string input)
        {
            int indexOfe = input.IndexOf('e');
            int indexOfE = input.IndexOf('E');

            if (input.StartsWith('e') || input.StartsWith('E'))
            {
                return false;
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input, i) && (i < indexOfe || i < indexOfE) && input[i] != '.')
                {
                    return false;
                }

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
            if (!char.IsDigit(input, input.Length - 1))
            {
                return false;
            }

            for (int j = nextElementAfterExponent; j < input.Length; j++)
            {
                if (!char.IsDigit(input, j) && input[j] != '-' && input[j] != '+')
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
