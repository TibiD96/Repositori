using System;

namespace JsonValidation
{
    public class Text : IPattern
    {
        private readonly string prefix;

        public Text(string prefix)
        {
            this.prefix = prefix;
        }

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new Match(false, text);
            }

            return text.Length < prefix.Length || text.Substring(0, prefix.Length) != prefix ? new Match(false, text) : new Match(true, text[prefix.Length..]);
        }
    }
}
