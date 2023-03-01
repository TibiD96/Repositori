using Xunit;

namespace JsonValidation
{
    public class OneOrMoreTests
    {
        [Fact]

        public void ReturnFalseIfInputIsNullOrEmpty()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.False(a.Match("").Success());
            Assert.Equal("", a.Match("").RemainingText());

            Assert.False(a.Match(null).Success());
            Assert.Null(a.Match(null).RemainingText());
        }

        [Fact]

        public void ReturnTrueIfAllCharsFromStringAreInsideRange()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.True(a.Match("123").Success());
            Assert.Equal("", a.Match("123").RemainingText());

        }

        [Fact]

        public void ReturnTrueIfJustSomeCharsFromStringAreInsideRangeAndStringStartsWithIt()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.True(a.Match("1a").Success());
            Assert.Equal("a", a.Match("1a").RemainingText());

        }

        [Fact]

        public void ReturnFalseIfStringDontContainCharsFromRange()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.False(a.Match("bc").Success());
            Assert.Equal("bc", a.Match("bc").RemainingText());

        }

        [Fact]

        public void ReturnFalseIfStringContainCharsFromRangeButDontStartWithIt()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.False(a.Match("b1").Success());
            Assert.Equal("b1", a.Match("b1").RemainingText());

        }
    }
}
