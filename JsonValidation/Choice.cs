using System;


namespace JsonValidation
{
    public class Choice : IPattern
    {
        private IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(string text)
        {
            IMatch match = new Match(true, text);

            foreach (var pattern in patterns)
            {
                match = pattern.Match(match.RemainingText());

                if (match.Success())
                {
                    return match;
                }
            }

            return new Match(false, text);
        }
    }
}