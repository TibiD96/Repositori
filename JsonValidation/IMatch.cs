using System;

namespace JsonValidation
{
    public interface IMatch
    {
        bool Success();
        string RemainingText();
    }
}