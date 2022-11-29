using System;
using System.Linq;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

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
            int index = input.Length - 1;
            const char controlCharacters = '\n';
            if (input.Contains('\\') || input.Contains(controlCharacters))
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (char.IsControl(input, i) || input[i] == 'x' || input[index - 1] == '\\')
                    {
                        return false;
                    }
                }

                return input[index - 1] != 'u' && !char.IsDigit(input[index - 1]);
            }

            return input.StartsWith('"') && input.EndsWith('"');
        }
    }
}
