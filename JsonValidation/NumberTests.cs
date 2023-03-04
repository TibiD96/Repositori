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
        }
    }
}
