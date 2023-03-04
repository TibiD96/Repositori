using System;

namespace JsonValidation
{
    public class Number : IPattern
    {
        private readonly IPattern pattern;

        public Number()
        {

            var zeroToNine = new Range('0', '9');
            this.pattern = new Sequence(zeroToNine);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}