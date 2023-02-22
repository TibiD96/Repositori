using System;

namespace JsonValidation
{
    class Sequence : IPattern
    {
        private IPattern[] patterns;
        public Sequence(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new Match(false, text);
            }

            return new Match(false, text);
        }
    }
}
