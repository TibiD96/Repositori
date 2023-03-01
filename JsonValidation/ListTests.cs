using Xunit;

namespace JsonValidation
{
    public class ListTests
    {
        [Fact]

        public void ReturnTrueWhenNullOrEmpty()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("").Success());
            Assert.Equal("", a.Match("").RemainingText());

            Assert.True(a.Match(null).Success());
            Assert.Null(a.Match(null).RemainingText());
        }

        [Fact]

        public void ReturnTrueAndConsumeTheCharsWhichAreEqualWithPattern()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("1,2,3").Success());
            Assert.Equal("", a.Match("1,2,3").RemainingText());
        }
    }
}
