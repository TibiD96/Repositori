using Xunit;

namespace JsonValidation
{
    public class ListTests
    {
        [Fact]

        public void ReturnTrueWhenNullOrEmpty()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("").Success());
            Assert.Equal("", a.Match("").RemainingText());

            Assert.True(a.Match(null).Success());
            Assert.Null(a.Match(null).RemainingText());
        }

        [Fact]

        public void ReturnTrueAndConsumeTheEntireStringIfItContainsJustTheList()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("1,2,3").Success());
            Assert.Equal("", a.Match("1,2,3").RemainingText());

            var digits = new OneOrMore(new Range('0', '9'));
            var whitespace = new Many(new Any(" \r\n\t"));
            var separator = new Sequence(whitespace, new Character(';'), whitespace);
            var list = new List(digits, separator);
            Assert.True(list.Match("1; 22  ;\n 333 \t; 22").Success());
            Assert.Equal("", list.Match("1; 22  ;\n 333 \t; 22").RemainingText());

        }

        [Fact]

        public void ReturnTrueAndConsumeJustAPartOfStringIfItContainsMoreThanJustTheList()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("1,2,3,").Success());
            Assert.Equal(",", a.Match("1,2,3,").RemainingText());

            Assert.True(a.Match("1a").Success());
            Assert.Equal("a", a.Match("1a").RemainingText());

            Assert.True(a.Match("1,2,3,").Success());
            Assert.Equal(",", a.Match("1,2,3,").RemainingText());

            var digits = new OneOrMore(new Range('0', '9'));
            var whitespace = new Many(new Any(" \r\n\t"));
            var separator = new Sequence(whitespace, new Character(';'), whitespace);
            var list = new List(digits, separator);
            Assert.True(list.Match("1 \n;").Success());
            Assert.Equal(" \n;", list.Match("1 \n;").RemainingText());
        }

        [Fact]

        public void ReturnTrueEvenIfTheStringDontContainAnyOfTheListElement()
        {
            var a = new List(new Range('0', '9'), new Character(','));
            Assert.True(a.Match("abc").Success());
            Assert.Equal("abc", a.Match("abc").RemainingText());

            var digits = new OneOrMore(new Range('0', '9'));
            var whitespace = new Many(new Any(" \r\n\t"));
            var separator = new Sequence(whitespace, new Character(';'), whitespace);
            var list = new List(digits, separator);
            Assert.True(list.Match("abc").Success());
            Assert.Equal("abc", list.Match("abc").RemainingText());
        }

    }
}
