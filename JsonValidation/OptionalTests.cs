using Xunit;

namespace JsonValidation
{
    public class OptionalTests
    {
        [Fact]

        public void ReturnTrueAndIfInputStartsWithGivenLetterItGetsConsumed()
        {
            var a = new Optional(new Character('a'));
            Assert.True(a.Match("abc").Success());
            Assert.Equal("bc", a.Match("abc").RemainingText());

            Assert.True(a.Match("aabc").Success());
            Assert.Equal("abc", a.Match("aabc").RemainingText());

            Assert.True(a.Match("bc").Success());
            Assert.Equal("bc", a.Match("bc").RemainingText());

            Assert.True(a.Match("").Success());
            Assert.Equal("", a.Match("").RemainingText());

            Assert.True(a.Match(null).Success());
            Assert.Null(a.Match(null).RemainingText());
        }

        [Fact]

        public void ReturnTrueAndIfInputStartsWithGivenSignItGetsConsumed()
        {
            var sign = new Optional(new Character('-'));
            Assert.True(sign.Match("123").Success());
            Assert.Equal("123", sign.Match("123").RemainingText());

            Assert.True(sign.Match("-123").Success());
            Assert.Equal("123", sign.Match("-123").RemainingText());
        }
    }
}
