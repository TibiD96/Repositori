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
            var integerPart = new Choice(new Sequence(oneToNine, numbers), removeOne);
            var sign = new Optional(new Any("+-"));

            var dot = new Character('.');
            var fractionalPart = new Optional(new Sequence(dot, numbers));

            var exponentLetter = new Any("eE");
            var exponentLetterAndSign = new Optional(new Sequence(exponentLetter, sign));
            var exponentFinal = new Optional(new Sequence(exponentLetter,sign, numbers));
            this.pattern = new Sequence(sign, integerPart, fractionalPart, exponentFinal);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
