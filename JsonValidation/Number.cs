using System;

namespace JsonValidation
{
    public class Number : IPattern
    {
        private readonly IPattern pattern;

        public Number()
        {

            var oneToNine = new Range('1', '9');
            var zero = new Character('0');
            var startingNumber = new Choice(zero, oneToNine);
            var number = new OneOrMore(startingNumber);
            var integer = new Choice(new Sequence(startingNumber, number));
            this.pattern = new Sequence(integer);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}