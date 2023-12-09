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

        public void CheckFirstUniqElementUniqLetterArePresent()
        {
            const string input = "aanbcnmbmxny";
            var result = StringFunctions.FirstUniqElement(input);
            Assert.Equal('c', result);
        }

        public void CheckFirstUniqElementNoUniqElements()
        {
            const string input = "aanbccnmbmxynyx";
            var result = StringFunctions.FirstUniqElement(input);
            Assert.Throws<InvalidOperationException>(() => StringFunctions.FirstUniqElement(input));
        }

        public void CheckStringToIntWhenInputIsValid()
        {
            const string input = "253";
            var result = StringFunctions.StringToInteger(input);
            Assert.Equal(253, result);
        }

        public void CheckStringToIntWhenInputIsInvalid()
        {
            const string input = "2A3";
            var result = StringFunctions.StringToInteger(input);
            Assert.Equal(253, result);
        }
    }
}
