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

            var array = ;
            var obj = ;
            pattern = value;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
