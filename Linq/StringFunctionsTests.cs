using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Linq
{
    public class StringFunctionsTests
    {
        [Fact]

        public void CheckVowelsAndConson()
        {
            const string input = "string";
            var result = StringFunctions.CountVowelsAndConsons(input);
            Assert.Equal((1, 5), result);
        }

        [Fact]

        public void CheckFirstUniqElementUniqLetterArePresent()
        {
            const string input = "aanbcnmbmxny";
            var result = StringFunctions.FirstUniqElement(input);
            Assert.Equal('c', result);
        }

        [Fact]

        public void CheckFirstUniqElementNoUniqElements()
        {
            const string input = "aanbccnmbmxynyx";
            Assert.Throws<InvalidOperationException>(() => StringFunctions.FirstUniqElement(input));
        }

        [Fact]

        public void CheckStringToIntWhenInputIsValid()
        {
            const string input = "253";
            var result = StringFunctions.StringToInteger(input);
            Assert.Equal(253, result);
        }

        [Fact]

        public void CheckStringToIntWhenInputIsInvalid()
        {
            const string input = "2A3";
            Assert.Throws<InvalidOperationException>(() => StringFunctions.StringToInteger(input));
        }

        [Fact]

        public void CheckCharacterWithMostAppearanceOneCharater()
        {
            const string input = "adfgdopfnf";
            var expected = new[] { 'f' };
            var result = StringFunctions.CharacterWithMostAppearances(input);
            Assert.Equal(expected, result);
        }

        [Fact]

        public void CheckCharacterWithMostAppearanceMultipleCharater()
        {
            const string input = "adfgdopfndf";
            var expected = new[] { 'f', 'd' };
            var result = StringFunctions.CharacterWithMostAppearances(input);
            Assert.Equal(expected, result);
        }
    }
}
