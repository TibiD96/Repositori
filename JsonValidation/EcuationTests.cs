using Xunit;

namespace JsonValidation
{
    public class EcuationTests
    {
        [Fact]

        public void ReturnTrueAndEmptyForACorrectSimpleEcuation()
        {
            var value = new Value();
            Assert.True(value.Match("1 + 1").Success());
            Assert.Equal("", value.Match("1 + 1").RemainingText());
        }
    }
}