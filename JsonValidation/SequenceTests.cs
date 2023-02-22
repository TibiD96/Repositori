using Xunit;

namespace JsonValidation
{
    public class SequenceTests
    {
        [Fact]
        public void ReturnFasleForNull()
        {
            string text = null;
            var ab = new Sequence(new Character('a'), new Character('b'));
            Assert.False(ab.Match(text).Success());
            Assert.Equal(text, ab.Match(text).RemainingText());
        }

        [Fact]
        public void ReturnFasleForEmpty()
        {
            string text = "";
            var ab = new Sequence(new Character('a'), new Character('b'));
            Assert.False(ab.Match(text).Success());
            Assert.Equal(text, ab.Match(text).RemainingText());
        }


    }
}
