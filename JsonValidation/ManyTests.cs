using Xunit;

namespace JsonValidation
{
    public class ManyTests
    {
        [Fact]

        public void ReturnTrueAndAsLongAsInputStartsWithGivenCharItGetsConsumed()
        {
            var a = new Many(new Character('a'));
            Assert.True(a.Match("abc").Success());
            Assert.Equal("bc", a.Match("abc").RemainingText());

            Assert.True(a.Match("aaabc").Success());
            Assert.Equal("bc", a.Match("aaaabc").RemainingText());

            Assert.True(a.Match("bc").Success());
            Assert.Equal("bc", a.Match("bc").RemainingText());

            Assert.True(a.Match("").Success());
            Assert.Equal("", a.Match("").RemainingText());

            Assert.True(a.Match(null).Success());
            Assert.Null(a.Match(null).RemainingText());
        }

        [Fact]

        public void ReturnTrueAndAsLongAsInputStartsWithACharWhichIsPartOfTangeItGetsConsumed()
        {
            var digits = new Many(new Range('0', '9'));
            Assert.True(digits.Match("12345ab123").Success());
            Assert.Equal("ab123", digits.Match("12345ab123").RemainingText());

            Assert.True(digits.Match("ab").Success());
            Assert.Equal("ab", digits.Match("ab").RemainingText());
        }
    }
}
