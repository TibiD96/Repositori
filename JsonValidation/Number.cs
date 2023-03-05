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
            var removeOne = new Choice(zero, oneToNine);
            var numbers = new OneOrMore(removeOne);
            var integer = new Choice(new Sequence(oneToNine, numbers), removeOne);
            this.pattern = new Sequence(integer);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}