using System;

namespace JsonValidation
{
    public class Number : IPattern
    {
        private readonly IPattern pattern;

        public Number()
        {

            var zero = new Character('0');
            var digit = new Choice(zero, new Range('1', '9'));
            var digits = new OneOrMore(digit);
            var minus = new Optional(new Character('-'));
            var integer = new Sequence(minus, new Choice(zero, digits));
            var sign = new Optional(new Any("+-"));

            var dot = new Character('.');
            var fractional = new Optional(new Sequence(dot, digits));

            var exponentLetter = new Any("eE");
            var exponent = new Optional(new Sequence(exponentLetter,sign, digits));
            this.pattern = new Sequence(integer, fractional, exponent);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
