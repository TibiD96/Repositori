using System;

namespace JsonValidation
{
    public class String : IPattern
    {
        private readonly IPattern pattern;

        public String()
        {
            var letters = new Range('a', 'z');
            var quotes = new Character('"');
            var backSlash = new Character('\\');
            var escapedCharacters = new Sequence(backSlash, new Choice(new Any("\"\\/bfnrtu"), letters));
            var stringChar = new Choice(new Range(' ', '!'), new Range('A', 'Z'), new Range('a', '~'), escapedCharacters);
            var character = new Many(stringChar);
            this.pattern = new Sequence(quotes, character, quotes);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
