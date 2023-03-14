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

        public void Add(IPattern pattern)
        {
            Array.Resize(ref patterns, patterns.Length + 1);
            for (int i = patterns.Length - 1; i > 0; i--)
            {
                patterns[i] = patterns[i - 1];
            }

            patterns[patterns.Length - 1] = pattern;
        }
    }
}