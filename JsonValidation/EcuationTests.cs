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
            Assert.Equal("", value.Match("1 + 1").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForAMoreComplexOperationWithOneNumber()
        {
            var value = new Ecuation();
            Assert.True(value.Match("1").Success());
            Assert.Equal("", value.Match("1 + 1").RemainingText());
        }
    }
}