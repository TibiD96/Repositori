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

        [Fact]

        public void ReturnEmptyStringIfStringIsJasonValid()
        {
            var word = new String();
            Assert.Equal("", word.Match(Quoted("qwerty")).RemainingText());


        }

        public static string Quoted(string text)
            => $"\"{text}\"";
    }
}
