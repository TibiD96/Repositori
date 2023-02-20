using System;

namespace JsonValidation
{
    internal interface IPattern
    {
        bool Match(string text);
    }
}