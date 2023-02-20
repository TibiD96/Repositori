using System;

namespace JsonValidation
{
    public class Range : IPattern
    {
        private readonly char start;
        private readonly char end;
        public Range(char start, char end)
        {
            this.start = start;
            this.end = end;
        }

        public bool Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (text[0] >= start && text[0] <= end)
            {
                return true;
            }

            return false;
        }
    }
}