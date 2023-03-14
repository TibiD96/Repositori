using Xunit;

namespace JsonValidation
{
    public class ValueTests
    {
        [Fact]

        public void ReturnTrueAndEmptyForValidString()
        {
            var value = new Value();
            Assert.True(value.Match(Quoted("asd")).Success());
            Assert.Equal("", value.Match(Quoted("asd")).RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForValidNumber()
        {
            var value = new Value();
            Assert.True(value.Match("123").Success());
            Assert.Equal("", value.Match("123").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForValidArray()
        {
            var value = new Value();
            Assert.True(value.Match("[ 123 ]").Success());
            Assert.Equal("", value.Match("[ 123 ]").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForValidObject()
        {
            var value = new Value();
            Assert.True(value.Match("{ \"stores\" : 1000 }").Success());
            Assert.Equal("", value.Match("{ \"stores\" : 1000 }").RemainingText());
        }

        public static string Quoted(string text)
           => $"\"{text}\"";
    }
}
