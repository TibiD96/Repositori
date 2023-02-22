using Xunit;

namespace JsonValidation
{
    public class RangeTests
    {
        [Fact]

        public void CheckIfTheGivenStringStartWithACorrectChartacter()
        {
            string text = "akljhbdofij";
            Range range = new Range('a', 'f');
            Assert.True(range.Match(text).Success());
            Assert.Equal(text[1..], range.Match(text).RemainingText());
        }

        [Fact]

        public void StringDontStartWithCharacterOutOfRange()
        {
            string text = "qkljhbdofij";
            Range range = new Range('a', 'f');
            Assert.False(range.Match(text).Success());
            Assert.Equal(text[1..], range.Match(text).RemainingText());
        }

        [Fact]

        public void StringIsNotNull()
        {
            string text = null;
            Range range = new Range('a', 'f');
            Assert.False(range.Match(text).Success());
            Assert.Equal(text, range.Match(text).RemainingText());
        }

        [Fact]

        public void StringIsNotEmpty()
        {
            string text = "";
            Range range = new Range('a', 'f');
            Assert.False(range.Match(text).Success());
            Assert.Equal(text, range.Match(text).RemainingText());
        }
    }
}