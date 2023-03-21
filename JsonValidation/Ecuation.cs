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
            var element = new Sequence(ws, value, ws);
            var elements = new Choice(new List(element, sign), new List(sign, element));

            var ecuation = new Sequence(ws, elements, ws);

            value.Add(ecuation);
            pattern = ecuation;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}