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

            return integerNumber.StartsWith('-')
                   ? IsDigits(integerNumber[1..])
                   : IsDigits(integerNumber);
        }

        private static bool IsFraction(string fractionalNumber)
        {
            return fractionalNumber == "" || IsDigits(fractionalNumber[1..]);
        }

        private static bool IsExponent(string exponentialNumber)
        {
            exponentialNumber = exponentialNumber.ToLower();

            if (exponentialNumber == "")
            {
                return true;
            }

            exponentialNumber = exponentialNumber[1..];

            return IsDigits(exponentialNumber.StartsWith('-') || exponentialNumber.StartsWith('+') ? exponentialNumber[1..] : exponentialNumber);
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
                if (indexOfExponent != -1 && indexOfExponent > indexOfDot)
                {
                    return input[indexOfDot..indexOfExponent];
                }

                return input[indexOfDot..];
            }

            return string.Empty;
        }

        static string Exponent(string input, int indexOfExponent)
        {
            if (indexOfExponent != -1)
            {
                return input[indexOfExponent..];
            }

            return string.Empty;
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
