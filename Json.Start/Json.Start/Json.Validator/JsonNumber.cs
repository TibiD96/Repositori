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

            return IsInteger(Integer(input, indexOfDot, indexOfExponent))
                   && IsFraction(Fraction(input, indexOfDot, indexOfExponent))
                   && IsExponent(Exponent(input, indexOfExponent));
        }

        private static bool IsInteger(string integerNumber)
        {
            if (integerNumber.StartsWith('0') && integerNumber.Length > 1)
            {
                return false;
            }

            if (integerNumber.StartsWith('-'))
            {
                return IsDigits(integerNumber[1..]);
            }

            return IsDigits(integerNumber);
        }

        private static bool IsFraction(string fractionalNumber)
        {
            if (fractionalNumber == "")
            {
                return true;
            }

            return IsDigits(fractionalNumber[1..]);
        }

        private static bool IsExponent(string exponentialNumber)
        {
            exponentialNumber = exponentialNumber.ToLower();

            if (exponentialNumber == "")
            {
                return true;
            }

            exponentialNumber = exponentialNumber.Remove(0, 1);

            if (exponentialNumber.StartsWith('-') || exponentialNumber.StartsWith('+'))
            {
                return IsDigits(exponentialNumber[1 ..]);
            }

            return IsDigits(exponentialNumber);
        }

        static string Integer(string input, int indexOfDot, int indexOfExponent)
        {
            if (indexOfDot != -1)
            {
                return input[..indexOfDot];
            }

            if (indexOfExponent != -1)
            {
                return input[..indexOfExponent];
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

                return input[indexOfDot..];
            }

            input = string.Empty;

            return input;
        }

        static string Exponent(string input, int indexOfExponent)
        {
            if (indexOfExponent != -1)
            {
                return input[indexOfExponent..];
            }

            input = string.Empty;

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
