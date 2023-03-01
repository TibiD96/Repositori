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
        }
    }
}
