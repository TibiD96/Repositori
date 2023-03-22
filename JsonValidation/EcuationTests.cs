using Xunit;

namespace JsonValidation
{
    public class EcuationTests
    {
        [Fact]

        public void ReturnTrueAndEmptyForACorrectSimpleEcuation()
        {
            var value = new Ecuation();
            Assert.True(value.Match("1 + 1").Success());
            Assert.Equal("", value.Match("1 + 1").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForAOperationWithOneNumber()
        {
            var value = new Ecuation();
            Assert.True(value.Match("1").Success());
            Assert.Equal("", value.Match("1").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForAMoreComplexOperationNoBrackets()
        {
            var value = new Ecuation();
            Assert.True(value.Match("1 + 1 + 1 / 2 - 5 * 4").Success());
            Assert.Equal("", value.Match("1 + 1 + 1 / 2 - 5 * 4").RemainingText());
        }

        [Fact]

        public void ReturnFalseAndReturnRemainingTextUncorrectEcuationNoBrackets()
        {
            var value = new Ecuation();
            Assert.True(value.Match("1 + 1 -").Success());
            Assert.Equal("-", value.Match("1 + 1 -").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForASimpelOperationWithBrackets()
        {
            var value = new Ecuation();
            Assert.True(value.Match("(1 + 1)").Success());
            Assert.Equal("", value.Match("(1 + 1)").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForEcuationWithOaneSetsOfBracketsAndComplexEcuation()
        {
            var value = new Ecuation();
            Assert.True(value.Match("(1 + 1 + 1)").Success());
            Assert.Equal("", value.Match("(1 + 1 + 1)").RemainingText());
        }
    }
}