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
            int letterCounter = 0;
            if (!char.IsDigit(input, input.Length - 1))
            {
                return false;
            }

            int indexOfExponent = input.IndexOf('e');
            int indexOfCapitalEExponent = input.IndexOf('E');
            int indexOfDot = input.IndexOf('.');

            if (input.Contains('e') && indexOfExponent < indexOfDot)
            {
                return false;
            }

            if (input.Contains('E') && indexOfCapitalEExponent < indexOfDot)
            {
                return false;
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLetter(input, i))
                {
                    letterCounter++;
                }

                if (letterCounter > 1)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool DoesNotContainLetters(string input)
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
