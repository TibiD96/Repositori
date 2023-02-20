using System;


namespace JsonValidation
{
    internal class Choice
    {
        private IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public bool Match(string text)
        {
            foreach (var pattern in patterns)
            {
                if (pattern.Match(text))
                {
                    return pattern.Match(text);
                }
            }

            return false;
        }
    }
}