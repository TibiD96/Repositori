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
            IMatch match = pattern.Match(text);
            return match.Success() ? new Match(true, match.RemainingText()) : new Match(true, text);
        }
    }
}
