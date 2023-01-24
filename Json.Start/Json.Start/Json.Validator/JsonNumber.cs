using System;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            var indexOfDot = input.IndexOf('.');
            const string exponentCharaters = "eE";
            var indexOfExponent = input.IndexOfAny(exponentCharaters.ToCharArray());

            return IsInteger(Integers(input, indexOfDot, indexOfExponent)) && IsFraction(Fraction(input, indexOfDot, indexOfExponent));
        }

        private static bool IsInteger(bool integers)
        {
            return integers;
        }

        private static bool IsFraction(bool fraction)
        {
            return fraction;
        }

        static bool Integers(string input, int indexOfDot, int indexOfExponent)
        {
            if (indexOfDot != -1 || indexOfExponent != -1)
            {
                return true;
            }

            if (input.Length == 1)
            {
                return IsDigit(input[0]);
            }

            if (input.StartsWith('0'))
            {
                return false;
            }

            for (int i = 0; i < input.Length && !input.StartsWith('-'); i++)
            {
              if (!IsDigit(input[i]))
              {
                 return false;
              }
            }

            for (int i = 1; i < input.Length; i++)
            {
              if (!IsDigit(input[i]))
              {
                  return false;
              }
            }

            return true;
        }

        static bool Fraction(string input, int indexOfDot, int indexOfExponent)
        {
            if (indexOfDot != -1 && input.Length > 1)
            {
                if (input.StartsWith('.') || input.EndsWith('.'))
                {
                    return false;
                }

                for (int i = indexOfDot + 1; i < input.Length; i++)
                {
                    if (!IsDigit(input[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return input.Length != 1 || IsDigit(input[0]);
        }

        static bool IsDigit(char input)
        {
            return char.IsDigit(input);
        }
    }
}
