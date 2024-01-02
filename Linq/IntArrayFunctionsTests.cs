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

        [Fact]

        public void ValidCombination()
        {
            const int lastNumber = 4;
            const int result = 0;
            var expected = new List<List<int>>()
            {
                new List<int> { 1, -2, -3, 4 },
                new List<int> { -1, 2, 3, -4 }
            };

            var final = IntArrayFunctions.ValidCombination(lastNumber, result);

            Assert.Equal(expected, final);
        }

        [Fact]

        public void ValidTriplets()
        {
            int[] inputArray = new[] { 13, 4, 12, 3, 5 };
            var expected = new List<List<int>>()
            {
                new List<int> { 3, 4, 5 },
                new List<int> { 5, 12, 13 }
            };

            var final = IntArrayFunctions.ValidTriplets(inputArray);

            Assert.Equal(expected, final);
        }
    }
}
