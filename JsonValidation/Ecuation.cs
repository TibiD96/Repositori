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
            var ws = new Many(new Character(' '));
            var sign = new Many(new Any("+-*/"));
            var openBrackets = new Character('(');
            var closeBrackets = new Character(')');
            var element = new Sequence(value, ws);
            var elements = new List(element, sign);

            var array = new Sequence(ws, elements, ws);

            value.Add(array);
            pattern = value;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}