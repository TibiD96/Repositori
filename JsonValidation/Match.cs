using System;

namespace JsonValidation
{
    internal class Match : IMatch
    {
        private readonly bool success;
        private readonly string remainingText;

        public Match(bool success, string remainigText)
        {
            this.success = success;
            this.remainingText = remainingText;
        }

        public bool Success()
        {
            return success;
        }

        public string RemainingText()
        {
            return remainingText;
        }
    }
}
