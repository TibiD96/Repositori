using Xunit;

namespace JsonValidation
{
    public class CharacterTests
    {
        [Fact]

        public void StringStartWithGivenChar()
        {
            string text = "sDgdfh";
            Character givenChar = new Character('s');
            Assert.True(givenChar.Match(text));
        }

        [Fact]

        public void StringStartWithGivenCharAndTheStringContainJustOneCharacter()
        {
            string text = "s";
            Character givenChar = new Character('s');
            Assert.True(givenChar.Match(text));
        }

        [Fact]

        public void StringIsNull()
        {
            string text = null;
            Character givenChar = new Character('s');
            Assert.False(givenChar.Match(text));
        }

        [Fact]

        public void StringIsEmpty()
        {
            string text = "";
            Character givenChar = new Character('s');
            Assert.False(givenChar.Match(text));
        }

        [Fact]

        public void StringDontStarttWithGivenChar()
        {
            string text = "fDgdfh";
            Character givenChar = new Character('s');
            Assert.False(givenChar.Match(text));
        }
    }
}