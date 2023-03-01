using System;

using JsonValidation;

class OneOrMore : IPattern
{
    private readonly IPattern pattern;

    public OneOrMore(IPattern pattern)
    {
        this.pattern = new Sequence(pattern);
    }

    public IMatch Match(string text)
    {
        return pattern.Match(text);
    }
}
