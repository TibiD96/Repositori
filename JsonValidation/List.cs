using System;

namespace JsonValidation
{
    public class List : IPattern
    {
        private readonly IPattern pattern;

        public List(IPattern element, IPattern separator)
        {
            this.pattern = new Optional(new Sequence(element, new Many(separator)));
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
