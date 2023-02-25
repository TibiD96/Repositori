using Xunit;

namespace JsonValidation
{
    public class AnyTests
    {
        [Fact]
        public void ReturnFasleForNullOrEmpty()
        {
            var eNull = new Any("eE");
            Assert.False(eNull.Match(null).Success());
            Assert.Equal(null, eNull.Match(null).RemainingText());
            var eEmpty = new Any("eE");
            Assert.False(eEmpty.Match("").Success());
            Assert.Equal("", eEmpty.Match("").RemainingText());
        }
    }
}
