using System;

namespace Json
{
    public static class JsonString
    {
        public static bool IsJsonString(string input)
        {
            int i = 0;
            int j = input.Length - 1;
            return input[i] == '"' && input[j] == '"';
        }
    }
}
