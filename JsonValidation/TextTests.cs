using Xunit;

namespace JsonValidation
{
    public class TextTests
    {
        [Fact]

        public void ReturnFalsForNullOrEmpty()
        {
            var boolTrue = new Text("true");
            Assert.False(boolTrue.Match(null).Success());
            Assert.Equal(null, boolTrue.Match(null).RemainingText());
            Assert.False(boolTrue.Match("").Success());
            Assert.Equal("", boolTrue.Match("").RemainingText());
            var boolFalse = new Text("false");
            Assert.False(boolFalse.Match(null).Success());
            Assert.Equal(null, boolFalse.Match(null).RemainingText());
            Assert.False(boolTrue.Match("").Success());
            Assert.Equal("", boolTrue.Match("").RemainingText());
        }
    }
}
