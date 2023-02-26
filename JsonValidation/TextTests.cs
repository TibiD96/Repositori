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

            Assert.False(boolFalse.Match("").Success());
            Assert.Equal("", boolFalse.Match("").RemainingText());
        }

        [Fact]

        public void ReturnTrueWhenTheInputConatinTheGivenString()
        {
            var boolTrue = new Text("true");
            Assert.True(boolTrue.Match("true").Success());
            Assert.Equal("", boolTrue.Match("true").RemainingText());

            Assert.True(boolTrue.Match("trueX").Success());
            Assert.Equal("X", boolTrue.Match("trueX").RemainingText());

            var boolFalse = new Text("false");
            Assert.True(boolFalse.Match("false").Success());
            Assert.Equal("", boolFalse.Match("false").RemainingText());

            Assert.True(boolFalse.Match("falseX").Success());
            Assert.Equal("X", boolFalse.Match("falseX").RemainingText());
        }

        [Fact]

        public void ReturnFalseWhenTheInputDontConatinTheGivenString()
        {
            var boolTrue = new Text("true");
            Assert.False(boolTrue.Match("false").Success());
            Assert.Equal("false", boolTrue.Match("false").RemainingText());

            var boolFalse = new Text("false");
            Assert.False(boolFalse.Match("true").Success());
            Assert.Equal("true", boolFalse.Match("true").RemainingText());
        }
    }
}
