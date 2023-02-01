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

        private static bool IsInteger(string integerNumber)
        {
            if (integerNumber.Length == 1)
            {
                return IsDigit(integerNumber);
            }

            if (integerNumber.StartsWith('0') && integerNumber[1] != '.')
            {
                return false;
            }

            if (integerNumber.StartsWith('-'))
            {
                integerNumber = integerNumber.Remove(0, 1);
            }

            return IsDigit(integerNumber);
        }

        private static bool IsFraction(string fractionalNumber)
        {
            if (fractionalNumber.Length == 1)
            {
                return IsDigit(fractionalNumber);
            }


            if (fractionalNumber.StartsWith('-'))
            {
                fractionalNumber = fractionalNumber.Remove(0, 1);
            }

            if (fractionalNumber.StartsWith('.'))
            {
                fractionalNumber = fractionalNumber.Remove(0, 1);
            }

            return IsDigit(fractionalNumber);
        }

        private static bool IsExponent(string exponentialNumber)
        {
            if (exponentialNumber.Length == 1)
            {
                return IsDigit(exponentialNumber);
            }

            if (exponentialNumber.StartsWith('-'))
            {
                exponentialNumber = exponentialNumber.Remove(0, 1);
            }

            if (exponentialNumber.StartsWith('e') && (exponentialNumber[1] == '-' || exponentialNumber[1] == '+'))
            {
                exponentialNumber = exponentialNumber.Remove(0, 1 + 1);

                return IsDigit(exponentialNumber);
            }

            if (exponentialNumber.StartsWith('e'))
            {
                exponentialNumber = exponentialNumber.Remove(0, 1);

                return IsDigit(exponentialNumber);
            }

            return IsDigit(exponentialNumber);
        }

        static string Integers(string input, int indexOfDot, int indexOfExponent)
        {
            if (input.Length == 1)
            {
                return input;
            }

            if (indexOfDot != -1)
            {
                return input.Substring(0, indexOfDot);
            }

            if (indexOfExponent != -1)
            {
                return input.Substring(0, indexOfExponent);
            }

            return input;
        }

        static string Fraction(string input, int indexOfDot, int indexOfExponent)
        {
            int countCharacters = 0;
            if (indexOfDot != -1 && indexOfExponent != -1)
            {
                for (int i = indexOfDot; i != indexOfExponent; i++)
                {
                    countCharacters++;
                }

                return input.Substring(indexOfDot, countCharacters);
            }

            if (indexOfDot != -1)
            {
                for (int i = indexOfDot; i < input.Length; i++)
                {
                    countCharacters++;
                }

                return input.Substring(indexOfDot, countCharacters);
            }

            return input;
        }

        static string Exponent(string input, int indexOfExponent)
        {
            input = input.ToLower();
            int countCharacters = 0;
            if (indexOfExponent != -1)
            {
                for (int i = indexOfExponent; i < input.Length; i++)
                {
                    countCharacters++;
                }

                return input.Substring(indexOfExponent, countCharacters);
            }

            return input;
        }

        static bool IsDigit(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
