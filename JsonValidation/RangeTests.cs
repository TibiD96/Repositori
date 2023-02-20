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
            Assert.True(range.Match(text));
        }

        [Fact]

        public void StringDontStartWithCharacterOutOfRange()
        {
            string text = "kkljhbdofij";
            Range range = new Range('a', 'f');
            Assert.False(range.Match(text));
        }

        [Fact]

        public void StringIsNotNull()
        {
            string text = null;
            Range range = new Range('a', 'f');
            Assert.False(range.Match(text));
        }

        [Fact]

        public void StringIsNotEmpty()
        {
            string text = "";
            Range range = new Range('a', 'f');
            Assert.False(range.Match(text));
        }
    }
}