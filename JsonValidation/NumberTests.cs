using Xunit;

namespace JsonValidation
{
    public class NumberTests
    {
        [Fact]

        public void ReturnFalseForNullOrEmpty()
        {
            var number = new Number();
            Assert.Equal("", number.Match("").RemainingText());
            Assert.Null(number.Match(null).RemainingText());
        }

        [Fact]

        public void RemoveTheStringIfIsIntegerAndHasJustOneNumber()
        {
            var number = new Number();
            Assert.Equal("", number.Match("1").RemainingText());
        }

        [Fact]

        public void RemoveTheStringIfIsInteger()
        {
            var number = new Number();
            Assert.Equal("", number.Match("123").RemainingText());
        }

        [Fact]

        public void RemoveTheStartOfStringIfIs0AndStringIsInteger()
        {
            var number = new Number();
            Assert.Equal("1", number.Match("01").RemainingText());
        }
    }
}
