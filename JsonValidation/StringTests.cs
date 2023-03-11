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
            var simpleString = new String();
            Assert.Equal("", simpleString.Match(Quoted("qwerty")).RemainingText());

            var stringWithEscapedQuotationMark = new String();
            Assert.Equal("", stringWithEscapedQuotationMark.Match(Quoted(@"\""a\"" b")).RemainingText());


        }

        public static string Quoted(string text)
            => $"\"{text}\"";
    }
}
