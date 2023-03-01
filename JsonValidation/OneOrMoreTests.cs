using Xunit;

namespace JsonValidation
{
    public class OneOrMoreTests
    {
        [Fact]

        public void ReturnFalseIfInputIsNullOrEmpty()
        {
            var a = new OneOrMore(new Range('0', '9'));
            Assert.False(a.Match("").Success());
            Assert.Equal("", a.Match("").RemainingText());

            Assert.False(a.Match(null).Success());
            Assert.Null(a.Match(null).RemainingText());
        }

     
    }
}
