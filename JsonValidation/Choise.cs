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

        public IMatch Match(string text)
        {
            foreach (var pattern in patterns)
            {
                if (pattern.Match(text).Success())
                {
                    return pattern.Match(text);
                }
            }

            return new Match(false, text);
        }
    }
}