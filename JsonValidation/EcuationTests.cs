using Xunit;

namespace JsonValidation
{
    public class EcuationTests
    {
        [Fact]

        public void ReturnTrueAndEmptyForACorrectSimpleEcuation()
        {
            var value = new Ecuation();
            Assert.True(value.Match("1 + 1").Success());
            Assert.Equal("", value.Match("1 + 1").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForAOperationWithOneNumber()
        {
            var value = new Ecuation();
            Assert.True(value.Match("1").Success());
            Assert.Equal("", value.Match("1").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForAMoreComplexOperationNoBrackets()
        {
            var value = new Ecuation();
            Assert.True(value.Match("1 + 1 + 1 / 2 - 5 * 4").Success());
            Assert.Equal("", value.Match("1 + 1 + 1 / 2 - 5 * 4").RemainingText());
        }

        [Fact]

        public void ReturnRemainingTextForInvalidEcuationNoBrackets()
        {
            var value = new Ecuation();
            Assert.True(value.Match("1 + 1 +").Success());
            Assert.Equal(" +", value.Match("1 + 1 +").RemainingText());

            Assert.True(value.Match("1 + 1 + - 1").Success());
            Assert.Equal(" + - 1", value.Match("1 + 1 + - 1").RemainingText());

            Assert.True(value.Match("- 1 + 1").Success());
            Assert.Equal("- 1 + 1", value.Match("- 1 + 1").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForASimpelOperationWithBrackets()
        {
            var value = new Ecuation();
            Assert.True(value.Match("( 1 + 1 )").Success());
            Assert.Equal("", value.Match("( 1 + 1 )").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForEcuationWithOaneLevelBrackets()
        {
            var value = new Ecuation();
            Assert.True(value.Match("( 1 + 1 + 1 )").Success());
            Assert.Equal("", value.Match("( 1 + 1 + 1 )").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForEcuationWithMoreLevelsBrackets()
        {
            var value = new Ecuation();
            Assert.True(value.Match("( 1 + 1 + 1 ) + ( 5 - 2 + 3 * 1 )").Success());
            Assert.Equal("", value.Match("( 1 + 1 + 1 ) + ( 5 - 2 + 3 * 1 )").RemainingText());

            Assert.True(value.Match("( 1 + 1 + 1 ) + ( ( 5 - 2 ) + ( 3 * 1 ) )").Success());
            Assert.Equal("", value.Match("( 1 + 1 + 1 ) + ( ( 5 - 2 ) + ( 3 * 1 ) )").RemainingText());
        }

        [Fact]

        public void ReturnTrueAndEmptyForComplexEcuationWithBrackets()
        {
            var value = new Ecuation();
            Assert.True(value.Match("( 1 + 1 + 1 ) + 5").Success());
            Assert.Equal("", value.Match("( 1 + 1 + 1 ) + 5").RemainingText());

            Assert.True(value.Match("( 1 + 1 + 1 ) + ( ( 5 - 2 ) * 10 )").Success());
            Assert.Equal("", value.Match("( 1 + 1 + 1 ) + ( ( 5 - 2 ) * 10 )").RemainingText());

            Assert.True(value.Match("( ( 5 + 10 ) )").Success());
            Assert.Equal("( ( 5 + 10 ) )", value.Match("( ( 5 + 10 ) )").RemainingText());
        }

        [Fact]

        public void ReturnRemainingTextForInvalidEcuationWithBrackets()
        {
            var value = new Ecuation();
            Assert.True(value.Match("( 1 + 1 + 1 ) - ( 10 + 5 ) -").Success());
            Assert.Equal(" -", value.Match("( 1 + 1 + 1 ) - ( 10 + 5 ) -").RemainingText());

            Assert.True(value.Match("+ ( 1 + 1 + 1 ) + ( ( 5 - 2 ) * 10 )").Success());
            Assert.Equal("+ ( 1 + 1 + 1 ) + ( ( 5 - 2 ) * 10 )", value.Match("+ ( 1 + 1 + 1 ) + ( ( 5 - 2 ) * 10 )").RemainingText());

            Assert.True(value.Match("( 1 + 1 + 1 ) + / 5)").Success());
            Assert.Equal(" + / 5 )", value.Match("( 1 + 1 + 1 ) + / 5 )").RemainingText());

            Assert.True(value.Match("( 1 + 1 + 1 ) * + 5").Success());
            Assert.Equal(" * + 5", value.Match("( 1 + 1 + 1 ) * + 5").RemainingText());

            Assert.True(value.Match("( 1 - 1 + - 1 ) * 5").Success());
            Assert.Equal("( 1 - 1 + - 1 ) * 5", value.Match("( 1 - 1 + - 1 ) * 5").RemainingText());

            Assert.True(value.Match("( 1 - 1 ) + / ( 100 * 200 ) + 5").Success());
            Assert.Equal(" + / ( 100 * 200 ) + 5", value.Match("( 1 - 1 ) + / ( 100 * 200 ) + 5").RemainingText());

            Assert.True(value.Match("( 5 + 10 (").Success());
            Assert.Equal("( 5 + 10 (", value.Match("( 5 + 10 (").RemainingText());

            Assert.True(value.Match("( 5 + 10 ) + ) 400 - 1000 (").Success());
            Assert.Equal(" + ) 400 - 1000 (", value.Match("( 5 + 10 ) + ) 400 - 1000 (").RemainingText());
        }

        [Fact]

        public void ReturnRemainingTextForANonEcuationString()
        {
            var value = new Ecuation();
            Assert.True(value.Match("abcd").Success());
            Assert.Equal("abcd", value.Match("abcd").RemainingText());

        }
    }
}