using System;

namespace JsonValidation
{
    public class Match : IMatch
    {
        private readonly bool success;
        private readonly string text;

        public Match(bool success, string text)
        {
            this.success = success;
            this.text = text;
        }

        public bool Success()
        {
            return success;
        }

        public string RemainingText()
        {
            return text;
        }
    }
}
