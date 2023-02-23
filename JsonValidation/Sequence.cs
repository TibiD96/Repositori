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
            string oldText = text;           

            foreach (var pattern in patterns)
            {
                if (!pattern.Match(text).Success())
                {
                    return new Match(false, oldText);
                }

                text = pattern.Match(text).RemainingText();
                
            }

            return new Match(true, text);
        }
    }
}
