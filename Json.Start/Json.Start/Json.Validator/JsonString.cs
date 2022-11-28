using System;

namespace Json
{
    public static class JsonString
    {
        public static bool IsJsonString(string input)
        {
            return !string.IsNullOrEmpty(input) && IsDoubleQuoted(input);
        }

        static bool IsDoubleQuoted(string input)
        {
            int controlIndex;
            const string backSlash = "\n";
            if (input.Contains(backSlash))
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (char.IsControl(input, i))
                    {
                        return false;
                    }
                }

                return true;
            }

            return input.StartsWith('"') && input.EndsWith('"');
        }
    }
}
