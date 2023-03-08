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
            var sign = new Optional(new Any("+-"));
            var dot = new Character('.');
            var fractional = new Optional(new Sequence(dot, numbers));
            this.pattern = new Sequence(sign, integer, fractional);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}