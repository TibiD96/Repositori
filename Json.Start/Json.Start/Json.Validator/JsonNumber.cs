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

            var indexOfDot = input.IndexOf('.');
            const string exponentCharaters = "eE";
            var indexOfExponent = input.IndexOfAny(exponentCharaters.ToCharArray());

            return IsInteger(Integers(input, indexOfDot, indexOfExponent)) && IsFraction(Fraction(input, indexOfDot, indexOfExponent)) && IsExponent(Exponent(input, indexOfExponent));
        }

        private static bool IsInteger(bool integers)
        {
            return integers;
        }

        private static bool IsFraction(bool fraction)
        {
            return fraction;
        }

        private static bool IsExponent(bool exponent)
        {
            return exponent;
        }

        static bool Integers(string input, int indexOfDot, int indexOfExponent)
        {
            if (input.Length == 1)
            {
                return IsDigit(input[0]);
            }

            if (input.StartsWith('0') && indexOfDot == -1)
            {
                return false;
            }

            if (input.StartsWith('-'))
            {
              return NumberIsNegative(input, indexOfDot, indexOfExponent);
            }

            return NumberIsPozitiv(input, indexOfDot, indexOfExponent);
        }

        static bool Fraction(string input, int indexOfDot, int indexOfExponent)
        {
          if (indexOfDot == input.Length - 1)
          {
            return false;
          }

          for (int i = indexOfDot + 1; i < input.Length && indexOfDot != -1; i++)
          {
            if (indexOfExponent != -1 && i == indexOfExponent)
            {
              return true;
            }

            if (!IsDigit(input[i]))
            {
              return false;
            }
          }

          return true;
        }

        static bool Exponent(string input, int indexOfExponent)
        {
            if (indexOfExponent == input.Length - 1)
            {
                return false;
            }

            for (int i = indexOfExponent + 1; i < input.Length && indexOfExponent != -1; i++)
            {
                if (i == indexOfExponent + 1 && (input[i] == '-' || input[i] == '+') && i != input.Length - 1)
                {
                    continue;
                }

                if (!IsDigit(input[i]))
                {
                    return false;
                }
            }

            return true;
        }

        static bool NumberIsNegative(string input, int indexOfDot, int indexOfExponent)
        {
            if (indexOfDot != -1)
            {
                for (int i = 1; i < indexOfDot; i++)
                {
                    if (!IsDigit(input[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            for (int i = 1; i < input.Length; i++)
            {
                if (indexOfExponent != -1 && i == indexOfExponent && i > 0)
                {
                    return true;
                }

                if (!IsDigit(input[i]))
                {
                    return false;
                }
            }

            return true;
        }

        static bool NumberIsPozitiv(string input, int indexOfDot, int indexOfExponent)
        {
            if (indexOfDot != -1)
            {
                for (int i = 0; i < indexOfDot; i++)
                {
                    if (!IsDigit(input[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (indexOfExponent != -1 && i == indexOfExponent && i > 0)
                {
                    return true;
                }

                if (!IsDigit(input[i]))
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsDigit(char input)
        {
            return char.IsDigit(input);
        }
    }
}
