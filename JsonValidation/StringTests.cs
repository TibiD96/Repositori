using Xunit;

namespace JsonValidation
{
    public class StringTests
    {
        [Fact]

        public void ReturnNullForNullAndEmptyForEmpty()
        {
            var word = new String();
            Assert.Equal("", word.Match("").RemainingText());
            Assert.Null(word.Match(null).RemainingText());
        }
    }
}
