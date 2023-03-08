using System;

namespace JsonValidation
{
    public class String : IPattern
    {
        private readonly IPattern pattern;

        public String()
        {
            var letters = new Range('a', 'z');
            this.pattern = new Sequence(letters);
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
