using System;

namespace JsonValidation
{
    public class Ecuation : IPattern
    {
        private readonly IPattern pattern;

        public Ecuation()
        {
            var number = new Number();
            var value = new Choice(number);
            var ws = new Many(new Any(" "));
            var sign = new Sequence(ws, new Any("+-*/"), ws);
            var openBrackets = new Character('(');
            var closeBrackets = new Character(')');
            var elements = new List(value, sign);

            var ecuation = new Sequence(openBrackets, ws, elements, ws, closeBrackets);

            value.Add(ecuation);
            pattern = elements;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}