using System;

namespace JsonValidation
{
    public class Value : IPattern
    {
        private readonly IPattern pattern;

        public Value()
        {
            var stringinput = new String();
            var number = new Number();
            var value = new Choice(stringinput, number, new Text("true"), new Text("false"), new Text("null"));
            var ws = new Many(new Any(" \r\n\t"));
            var squareBrackets = new Many(new Any("[]"));
            var element = new Sequence(ws, value, ws);
            var elements = new List(element, new Character(','));

            var array = new Sequence(squareBrackets, elements, squareBrackets);
            // var obj = ;
            value.Add(array);
            //value.Add(obj);
            pattern = value;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
