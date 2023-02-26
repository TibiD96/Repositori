using Xunit;

namespace JsonValidation
{
    public class AnyTests
    {
        [Fact]
        public void ReturnFasleForNullOrEmpty()
        {
            var e = new Any("eE");
            Assert.False(e.Match(null).Success());
            Assert.Null(e.Match(null).RemainingText());
            Assert.False(e.Match("").Success());
            Assert.Equal("", e.Match("").RemainingText());
            var sign = new Any("-+");
            Assert.False(sign.Match(null).Success());
            Assert.Null(sign.Match(null).RemainingText());
            Assert.False(sign.Match("").Success());
            Assert.Equal("", sign.Match("").RemainingText());
        }

        [Fact]
        public void ReturnTrueWhenTextStartsWithRightChar()
        {
            var e = new Any("eE");
            Assert.True(e.Match("ea").Success());
            Assert.Equal("a", e.Match("ea").RemainingText());
            Assert.True(e.Match("Ea").Success());
            Assert.Equal("a", e.Match("Ea").RemainingText());
            var sign = new Any("-+");
            Assert.True(sign.Match("+3").Success());
            Assert.Equal("3", sign.Match("+3").RemainingText());
            Assert.True(sign.Match("-2").Success());
            Assert.Equal("2", sign.Match("-2").RemainingText());
        }

        [Fact]
        public void ReturnFalseWhenTextDontStartsWithRightChar()
        {
            var e = new Any("eE");
            Assert.False(e.Match("a").Success());
            Assert.Equal("a", e.Match("a").RemainingText());
            Assert.False(e.Match("a").Success());
            Assert.Equal("a", e.Match("a").RemainingText());
            var sign = new Any("-+");
            Assert.False(sign.Match("2").Success());
            Assert.Equal("2", sign.Match("2").RemainingText());
            Assert.False(sign.Match("2").Success());
            Assert.Equal("2", sign.Match("2").RemainingText());
        }
    }
}
