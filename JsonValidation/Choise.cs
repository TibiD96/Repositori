using System;


namespace JsonValidation
{
    public class Choice
    {
        private IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new Match(false, text);
            }

            foreach (var pattern in patterns)
            {
                if (pattern.Match(text).Success())
                {
                    return new Match(true, text[1..]);
                }
            }

            return new Match(false, text[1..]);
        }
    }
}