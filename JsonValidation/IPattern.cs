using System;

namespace JsonValidation
{
    public interface IPattern
    {
        IMatch Match(string text);
    }
}