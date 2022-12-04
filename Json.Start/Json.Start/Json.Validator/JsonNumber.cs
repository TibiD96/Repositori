using System;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            return !string.IsNullOrEmpty(input) && IsNumber(input);
        }

        public static bool IsNumber(string input)
        {
            if (input.Contains('e') || input.Contains('E'))
            {
                return StringContainExponent(input);
            }

            if (input.Contains('.'))
            {
                return StringContainDot(input);
            }

            if (input.StartsWith('0') && input.Length > 1)
            {
                return false;
            }

            return DoesNotContainLetters(input);
        }

        public static bool StringContainDot(string input)
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

                if (dotCounter > 1)
                {
                    return false;
                }
            }

            return DoesNotContainLetters(input);
        }

        public static bool StringContainExponent(string input)
        {
            int exponentCounter = 0;
            if (input.EndsWith('e') || input.EndsWith('-') || input.EndsWith('+'))
            {
                return false;
            }

            int indexOfExponent = input.IndexOf('e');
            int indexOfDot = input.IndexOf('.');

            if (input.Contains('e') && input.Contains('.') && indexOfExponent < indexOfDot)
            {
                return false;
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == 'e')
                {
                    exponentCounter++;
                }

                if (exponentCounter > 1)
                {
                    return false;
                }
            }

            return DoesNotContainLetters(input);
        }

        public static bool DoesNotContainLetters(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
              if (!char.IsDigit(input, i) && input[i] != '.' && input[i] != 'e')
              {
                    return input[i] == 'E' || input[i] == '+' || input[i] == '-';
                }
            }

            return true;
        }
    }
}
