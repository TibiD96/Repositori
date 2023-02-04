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

            if (!IsInteger(Integer(input, indexOfDot, indexOfExponent)))
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
            if (integerNumber.StartsWith('0') && integerNumber.Length > 1)
            {
                return false;
            }

            if (integerNumber.StartsWith('-'))
            {
                integerNumber = integerNumber.Remove(0, 1);
            }

            return IsDigits(integerNumber);
        }

        private static bool IsFraction(string fractionalNumber)
        {
            if (fractionalNumber.StartsWith('-'))
            {
                fractionalNumber = fractionalNumber.Remove(0, 1);
            }

            if (fractionalNumber.StartsWith('.'))
            {
                return IsDigits(fractionalNumber[1..]);
            }

            return IsDigits(fractionalNumber);
        }

        private static bool IsExponent(string exponentialNumber)
        {
            exponentialNumber = exponentialNumber.ToLower();

            if (exponentialNumber.StartsWith('e'))
            {
                exponentialNumber = exponentialNumber.Remove(0, 1);
            }

            if (exponentialNumber.StartsWith('-') || exponentialNumber.StartsWith('+'))
            {
                return IsDigits(exponentialNumber[1..]);
            }

            return IsDigits(exponentialNumber);
        }

        static string Integer(string input, int indexOfDot, int indexOfExponent)
        {
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
            if (indexOfDot != -1)
            {
                int lengthOfSubstring;
                if (indexOfExponent != -1 && indexOfExponent > indexOfDot)
                {
                    lengthOfSubstring = indexOfExponent - indexOfDot;

                    return input.Substring(indexOfDot, lengthOfSubstring);
                }

                lengthOfSubstring = input.Length - indexOfDot;

                return input.Substring(indexOfDot, lengthOfSubstring);
            }

            if (indexOfExponent != -1)
            {
                return input.Substring(0, indexOfExponent);
            }

            return input;
        }

        static string Exponent(string input, int indexOfExponent, int indexOfDot)
        {
            int lengthOfSubstring;
            if (indexOfExponent != -1)
            {
                lengthOfSubstring = input.Length - indexOfExponent;

                return input.Substring(indexOfExponent, lengthOfSubstring);
            }

            if (indexOfDot != -1)
            {
                return input.Substring(0, indexOfDot);
            }

            return input;
        }

        static bool IsDigits(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return input.Length > 0;
        }
    }
}
