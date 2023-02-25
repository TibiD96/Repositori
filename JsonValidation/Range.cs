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

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new Match(false, text);
            }

            return text[0] >= start && text[0] <= end ? new Match(true, text[1..]) : new Match(false, text);
        }
    }
}