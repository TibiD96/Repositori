using System;

namespace JsonValidation
{
    public class Many : IPattern
    {
        private readonly IPattern pattern;

        public Many(IPattern pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            IMatch variableMatch = new Match(true, text);

            while (variableMatch.Success())
            {
                variableMatch = pattern.Match(variableMatch.RemainingText());
            }

            return new Match(true, variableMatch.RemainingText());
        }
    }
}
