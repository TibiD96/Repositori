using System;

namespace JsonValidation
{
    public class String : IPattern
    {
        private readonly IPattern pattern;

        public String()
        {
            var quotes = new Character('"');
            var hex = new Choice(new Range('a', 'f'), new Range('A', 'F'), new Range('0', '9'));
            var unicode = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            var escapedCharacters = new Sequence(new Character('\\'), new Choice(new Any("\"\\/bfnrtu"),unicode));
            var character = new Choice(new Range(' ', '!'), new Range('#', '['), new Range(']', char.MaxValue), escapedCharacters);
            var characters = new Many(character);
            this.pattern = new Sequence(quotes, characters, quotes);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
