using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Linq
{
    public class IntArrayFunctionsTests
    {
        [Fact]

        public void SumLowerEqualThenK()
        {
            int[] arrayInt = { 1, 2, 3, 4, 6 };
            const int number = 10;
            var expected = new List<List<int>>()
            {
                new List<int> { 1 },
                new List<int> { 1, 2 },
                new List<int> { 1, 2, 3 },
                new List<int> { 1, 2, 3, 4 },
                new List<int> { 2 },
                new List<int> { 2, 3 },
                new List<int> { 2, 3, 4 },
                new List<int> { 3 },
                new List<int> { 3, 4 },
                new List<int> { 4 },
                new List<int> { 4, 6 },
                new List<int> { 6 }
            };

            var result = IntArrayFunctions.SumLowerEqualThenK(arrayInt, number);

            Assert.Equal(expected, result);
        }
    }
}
