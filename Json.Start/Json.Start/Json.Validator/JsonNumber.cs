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

            if (!IsInteger(Integers(input, indexOfDot, indexOfExponent)))
            {
                return false;
            }

            if (!IsFraction(Fraction(input, indexOfDot, indexOfExponent)))
            {
                return false;
            }

            return IsExponent(Exponent(input, indexOfExponent, indexOfDot));
        }

        private static bool IsInteger(string integerNumber)
        {
            if (integerNumber.Length == 1)
            {
                return IsDigit(integerNumber);
            }

            if (string.IsNullOrEmpty(integerNumber))
            {
                return false;
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

            if (!fractionalNumber.Contains('.'))
            {
                return true;
            }

            fractionalNumber = fractionalNumber.Remove(0, 1);
            return IsDigit(fractionalNumber);
        }

        private static bool IsExponent(string exponentialNumber)
        {
            if (exponentialNumber.Length == 1)
            {
                return IsDigit(exponentialNumber);
            }

            if (exponentialNumber.StartsWith('e'))
            {
                exponentialNumber = exponentialNumber.Remove(0, 1);
            }

            const string plusMinusSign = "-+";
            var indexPlusMinusSign = exponentialNumber.IndexOfAny(plusMinusSign.ToCharArray());

            if (indexPlusMinusSign == 0 && exponentialNumber.Length > 1)
            {
                exponentialNumber = exponentialNumber.Remove(0, 1);
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
            input = input.ToLower();
            if (indexOfDot != -1)
            {
                for (int i = indexOfDot; i < input.Length && input[i] != 'e'; i++)
                {
                    countCharacters++;
                }

                return input.Substring(indexOfDot, countCharacters);
            }
            else if (indexOfExponent != -1)
            {
                return input.Substring(0, indexOfExponent);
            }

            return input;
        }

        static string Exponent(string input, int indexOfExponent, int indexOfDot)
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
            else if (indexOfDot != -1)
            {
                return input.Substring(0, indexOfDot);
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
