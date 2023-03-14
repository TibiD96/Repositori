using System;

namespace JsonValidation
{
    public class Value : IPattern
    {
        private readonly IPattern pattern;

        public Value()
        {
            var stringInput = new String();
            var number = new Number();
            var value = new Choice(stringInput, number, new Text("true"), new Text("false"), new Text("null"));
            var ws = new Many(new Any(" \r\n\t"));
            var openSquareBracket = new Character('[');
            var closeSquareBracket = new Character(']');
            var brace = new Many(new Any("{}"));
            var element = new Sequence(ws, value, ws);
            var elements = new List(element, new Character(','));
            var member = new Sequence(ws, stringInput, ws, new Character(':'), element);
            var members = new List(member, new Character(','));

            var obj = new Sequence(brace, members, brace);
            var array = new Sequence(openSquareBracket, elements, closeSquareBracket);

            value.Add(obj);
            value.Add(array);
            pattern = value;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
