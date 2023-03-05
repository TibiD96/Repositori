using Xunit;

namespace JsonValidation
{
    public class NumberTests
    {
        [Fact]

        public void ReturnFalseForNullOrEmpty()
        {
            var number = new Number();
            Assert.False(number.Match("").Success());
            Assert.Equal("", number.Match("").RemainingText());

            Assert.False(number.Match(null).Success());
            Assert.Null(number.Match(null).RemainingText());
        }

        [Fact]

        public void ReturnTrueForSingleNumber()
        {
            var number = new Number();
            Assert.True(number.Match("1").Success());
            Assert.Equal("", number.Match("1").RemainingText());
        }

        [Fact]

        public void ReturnTrueForMultipleNumbersString()
        {
            var number = new Number();
            Assert.True(number.Match("123").Success());
            Assert.Equal("", number.Match("123").RemainingText());
        }

        [Fact]

        public void ReturnFalseForIntergerWhichStrtWithZero()
        {
            var number = new Number();
            Assert.False(number.Match("01").Success());
            Assert.Equal("01", number.Match("01").RemainingText());
        }
    }
}
