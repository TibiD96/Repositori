using System;

namespace JsonValidation
{
    public class Optional : IPattern
    {
        private readonly IPattern pattern;

        public Optional(IPattern pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text).Success() ? new Match(true, text[1..]) : new Match(true, text);
        }
    }
}
