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
            if (input.Contains('.'))
            {
                return StringContainDot(input);
            }

            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input, i) && input[i] != '-')
                {
                    return false;
                }
            }

            return input.Length <= 1 || !input.StartsWith('0');
        }

        public static bool StringContainDot(string input)
        {
            if (input.EndsWith('.'))
            {
                return false;
            }

            return true;
        }
    }
}
