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
            IMatch variableMatch = new Match(true, text);           

            foreach (var pattern in patterns)
            {
                variableMatch = pattern.Match(variableMatch.RemainingText());

                if (!variableMatch.Success())
                {
                    return new Match(false, text);
                }
                
            }

            return variableMatch;
        }
    }
}
