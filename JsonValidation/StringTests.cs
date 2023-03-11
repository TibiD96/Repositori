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

            var stringWithEscapedReverseSolidus = new String();
            Assert.Equal("", stringWithEscapedReverseSolidus.Match(Quoted(@"a \\ b")).RemainingText());

            var stringWithEscapedSolidus = new String();
            Assert.Equal("", stringWithEscapedSolidus.Match(Quoted(@"a \/ b")).RemainingText());

            var stringWithEscapedBackSpace = new String();
            Assert.Equal("", stringWithEscapedBackSpace.Match(Quoted(@"a \b b")).RemainingText());

            var stringWithEscapedFormFeed = new String();
            Assert.Equal("", stringWithEscapedFormFeed.Match(Quoted(@"a \f b")).RemainingText());

            var stringWithEscapedLineFeed = new String();
            Assert.Equal("", stringWithEscapedLineFeed.Match(Quoted(@"a \n b")).RemainingText());

            var stringWithEscapedCarigeReturn = new String();
            Assert.Equal("", stringWithEscapedCarigeReturn.Match(Quoted(@"a \r b")).RemainingText());

            var stringWithEscapedHorizontalTab = new String();
            Assert.Equal("", stringWithEscapedHorizontalTab.Match(Quoted(@"a \t b")).RemainingText());
        }

        public static string Quoted(string text)
            => $"\"{text}\"";
    }
}
