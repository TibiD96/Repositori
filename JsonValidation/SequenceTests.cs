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
            var abc = new Sequence(ab, new Character('c'));
            Assert.False(abc.Match(text).Success());
            Assert.Equal(text, abc.Match(text).RemainingText());
        }

        [Fact]
        public void ReturnFasleForEmpty()
        {
            string text = "";
            var ab = new Sequence(new Character('a'), new Character('b'));
            Assert.False(ab.Match(text).Success());
            Assert.Equal(text, ab.Match(text).RemainingText());
            var abc = new Sequence(ab, new Character('c'));
            Assert.False(abc.Match(text).Success());
            Assert.Equal(text, abc.Match(text).RemainingText());
        }

        [Fact]
        public void ReturnTrueIfStringContainGivenChar()
        {
            string text = "abcd";
            var ab = new Sequence(new Character('a'), new Character('b'));
            Assert.True(ab.Match(text).Success());
            Assert.Equal("cd", ab.Match(text).RemainingText());
            var abc = new Sequence(ab, new Character('c'));
            Assert.True(abc.Match(text).Success());
            Assert.Equal("d", abc.Match(text).RemainingText());

        }

        [Fact]
        public void ReturnFalseIfStringDontContainAllOfGivenChar()
        {
            var ab = new Sequence(new Character('a'), new Character('b'));
            Assert.False(ab.Match("ax").Success());
            Assert.Equal("ax", ab.Match("ax").RemainingText());
            var abc = new Sequence(ab, new Character('c'));
            Assert.False(abc.Match("abx").Success());
            Assert.Equal("abx", abc.Match("abx").RemainingText());


        }

        [Fact]
        public void ReturnFalseIfStringDontContainGivenChar()
        {
            string text = "def";
            var ab = new Sequence(new Character('a'), new Character('b'));
            Assert.False(ab.Match(text).Success());
            Assert.Equal("def", ab.Match(text).RemainingText());
            var abc = new Sequence(ab, new Character('c'));
            Assert.False(abc.Match(text).Success());
            Assert.Equal("def", abc.Match(text).RemainingText());

        }

        [Fact]
        public void ReturnTrueIfContainAValidUnicode()
        {
            
            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            Assert.True(hexSeq.Match("u1234").Success());
            Assert.Equal("", hexSeq.Match("u1234").RemainingText());
            Assert.True(hexSeq.Match("uabcdef").Success());
            Assert.Equal("ef", hexSeq.Match("uabcdef").RemainingText());
            Assert.True(hexSeq.Match("uB005 ab").Success());
            Assert.Equal(" ab", hexSeq.Match("uB005 ab").RemainingText());

        }

        [Fact]
        public void ReturnFalseIfDontContainAValidUnicode()
        {

            var hex = new Choice(new Range('0', '9'), new Range('a', 'f'), new Range('A', 'F'));
            var hexSeq = new Sequence(new Character('u'), new Sequence(hex, hex, hex, hex));
            Assert.False(hexSeq.Match("abc").Success());
            Assert.Equal("abc", hexSeq.Match("abc").RemainingText());
            Assert.False(hexSeq.Match(null).Success());
            Assert.Equal(null, hexSeq.Match(null).RemainingText());


        }


    }
}
