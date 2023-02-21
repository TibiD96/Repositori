using Xunit;

namespace JsonValidation
{
    public class ChoiseTests
    {
        [Fact]

        public void ReturnTrueWhenStringIsContainJustDigits()
        {
            string input = "123";
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            Assert.True(digit.Match(input).Success());
        }

        [Fact]

        public void ReturnFalseWhenStringIsNull()
        {
            string input = null;
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            Assert.False(digit.Match(input).Success());
        }

        [Fact]

        public void ReturnFalseWhenStringIsEmpty()
        {
            string input = "";
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            Assert.False(digit.Match(input).Success());
        }

        [Fact]

        public void ReturnFalseWhenStringContainLetter()
        {
            string input = "a986";
            var digit = new Choice(new Character('0'), new Range('1', '9'));
            Assert.False(digit.Match(input).Success());
        }

        [Fact]

        public void ReturnTrueWhenStringContainLetter()
        {
            string input = "a986";
            var hex = new Choice(new Character('0'), new Range('1', '9'), new Range('a', 'f'), new Range('A', 'F'));
            Assert.True(hex.Match(input).Success());
        }

        [Fact]

        public void ReturnTrueWhenStringContainUpperCaseLetter()
        {
            string input = "A986";
            var hex = new Choice(new Character('0'), new Range('1', '9'), new Range('a', 'f'), new Range('A', 'F'));
            Assert.True(hex.Match(input).Success());
        }

        [Fact]

        public void ReturnFalseWhenStringContainLowerCaseLetterOutOfRange()
        {
            string input = "g986";
            var hex = new Choice(new Character('0'), new Range('1', '9'), new Range('a', 'f'), new Range('A', 'F'));
            Assert.False(hex.Match(input).Success());
        }

        [Fact]

        public void ReturnFalseWhenStringContainUpperCaseLetterOutOfRange()
        {
            string input = "G986";
            var hex = new Choice(new Character('0'), new Range('1', '9'), new Range('a', 'f'), new Range('A', 'F'));
            Assert.False(hex.Match(input).Success());
        }
    }
}