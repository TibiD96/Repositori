using System;

namespace JsonValidation
{
    public class Number : IPattern
    {
        private readonly IPattern pattern;

        public Number()
        {

            var zeroToNine = new Range('0', '9');
            var numbers = new OneOrMore(zeroToNine);
            this.pattern = new Sequence(numbers);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}