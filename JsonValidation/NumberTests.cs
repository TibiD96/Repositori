using Xunit;

namespace JsonValidation
{
    public class NumberTests
    {
        [Fact]

        public void ReturnNullForNullAndEmptyForEmpty()
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

        [Fact]

        public void RemoveTheStringIfIsNegativeInteger()
        {
            var number = new Number();
            Assert.Equal("", number.Match("-10").RemainingText());
        }

        [Fact]

        public void RemoveTheStringIfIsFractionalNumber()
        {
            var number = new Number();
            Assert.Equal("", number.Match("0.56").RemainingText());
        }

        [Fact]

        public void RemoveTheStringIfIsExponentialNumber()
        {
            var number = new Number();
            Assert.Equal("", number.Match("12e-3").RemainingText());
        }

        [Fact]

        public void RemoveTheStringIfIsExponentialNumberAndFractional()
        {
            var number = new Number();
            Assert.Equal("", number.Match("12.25e-3").RemainingText());
        }

        [Fact]

        public void ReturnStringIfItContainsJustLetters()
        {
            var number = new Number();
            Assert.Equal("qwerty", number.Match("qwerty").RemainingText());
        }

        [Fact]

        public void RemoveTheStringIfIs0()
        {
            var number = new Number();
            Assert.Equal("", number.Match("0").RemainingText());
        }

        [Fact]

        public void RemoveTheStringIfNegativValidNumber()
        {
            var number = new Number();
            Assert.Equal("", number.Match("-100").RemainingText());
        }

        [Fact]

        public void RemoveTheStringIfNegativ0()
        {
            var number = new Number();
            Assert.Equal("", number.Match("-0").RemainingText());
        }

        [Fact]

        public void RemoveTheIntegerPartOfInvalidFrationalNumber()
        {
            var number = new Number();
            Assert.Equal(".", number.Match("123.").RemainingText());
        }

        [Fact]

        public void RemoveTheValidPartOfFrationalNumber()
        {
            var number = new Number();
            Assert.Equal(".567", number.Match("123.54.567").RemainingText());
        }

        [Fact]

        public void RemoveTheValidPartOfExponentialNumber()
        {
            var number = new Number();
            Assert.Equal("e567", number.Match("123e54e567").RemainingText());
        }
    }
}
