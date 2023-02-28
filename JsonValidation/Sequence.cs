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
            IMatch match = new Match(true, text);           

            foreach (var pattern in patterns)
            {
                match = pattern.Match(match.RemainingText());

                if (!match.Success())
                {
                    return new Match(false, text);
                }
                
            }

            return match;
        }
    }
}
