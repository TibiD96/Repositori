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

        [Fact]

        public void ReturnTrueAndEmptyForValidComplexObject()
        {
            var value = new Value();
            Assert.True(value.Match("{ \"brand\" : \"nike\", \"financial\" : { \"stores\" : 1000, \"profit\" : 1000000, \"categoty\" : \"sport equipment\" }}").Success());
            Assert.Equal("", value.Match("{ \"brand\" : \"nike\", \"financial\" : { \"stores\" : 1000, \"profit\" : 1000000, \"categoty\" : \"sport equipment\" }}").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForValidComplexArray()
        {
            var value = new Value();
            Assert.True(value.Match("[ 123, 256, 1, 100 ]").Success());
            Assert.Equal("", value.Match("[ 123, 256, 1, 100 ]").RemainingText());
        }

        public static string Quoted(string text)
           => $"\"{text}\"";
    }
}
