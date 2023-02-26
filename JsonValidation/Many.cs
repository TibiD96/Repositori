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
            while (pattern.Match(text).Success())
            {
                text = text[1..];
            }

            return new Match(true, text);
        }
    }
}
