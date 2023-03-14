using System;

namespace JsonValidation
{
    public class Value : IPattern
    {
        private readonly IPattern pattern;

        public Value()
        {

            pattern = ;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
