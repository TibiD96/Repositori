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
    }
}
