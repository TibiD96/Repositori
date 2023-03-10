using System;

namespace JsonValidation
{
    public class String : IPattern
    {
        private readonly IPattern pattern;

        public String()
        {
            var letters = new Many(new Range('a', 'z'));
            var quotes = new Character('"');
            this.pattern = new Sequence(quotes, letters, quotes);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
