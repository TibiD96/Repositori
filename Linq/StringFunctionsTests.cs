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
    }
}
