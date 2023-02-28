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
            return !string.IsNullOrEmpty(text) && prefix.Length <= text.Length && text.StartsWith(prefix) 
                    ? new Match(true, text[prefix.Length..]) 
                     : new Match(false, text);
        }
    }
}
