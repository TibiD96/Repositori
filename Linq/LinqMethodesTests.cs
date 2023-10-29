using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Linq;
using Xunit;

namespace Linq
{
    public class LinqMethodesTests
    {
        [Fact]

        public void CheckAllMethode()
        {
            var first = new int[4];
            first[0] = 1;
            first[1] = 3;
            first[2] = 5;
            first[3] = 7;

            var second = new int[4];
            second[0] = 2;
            second[1] = 4;
            second[2] = 6;
            second[3] = 8;

            Assert.True(first.All(x => x % 2 == 1));
            Assert.False(second.All(x => x % 2 == 1));
            Assert.Throws<ArgumentNullException>(() => LinqMethodes.All<int>(null, x => x % 2 == 1));
        }

        [Fact]

        public void CheckAnyMethode()
        {
            var first = new int[4];
            first[0] = 2;
            first[1] = 3;
            first[2] = 4;
            first[3] = 8;

            var second = new int[4];
            second[0] = 2;
            second[1] = 4;
            second[2] = 6;
            second[3] = 8;

            Assert.True(first.Any(x => x % 2 == 1));
            Assert.False(second.Any(x => x % 2 == 1));
            Assert.Throws<ArgumentNullException>(() => LinqMethodes.Any<int>(null, x => x % 2 == 1));
            Assert.Throws<ArgumentNullException>(() => first.Any(null));
        }

        [Fact]

        public void CheckFirstMethode()
        {
            var first = new int[4];
            first[0] = 2;
            first[1] = 3;
            first[2] = 4;
            first[3] = 8;

            var second = new int[4];
            second[0] = 2;
            second[1] = 4;
            second[2] = 6;
            second[3] = 8;

            Assert.Equal(first[1], first.First(x => x % 2 == 1));
            Assert.Throws<InvalidOperationException>(() => second.First(x => x % 2 == 1));
            Assert.Throws<ArgumentNullException>(() => LinqMethodes.First<int>(null, x => x % 2 == 1));
            Assert.Throws<ArgumentNullException>(() => first.First(null));
        }

        [Fact]

        public void CheckSelectMethode()
        {
            var first = new int[4];
            first[0] = 2;
            first[1] = 3;
            first[2] = 4;
            first[3] = 8;

            var final = new int[4];
            final[0] = 4;
            final[1] = 9;
            final[2] = 16;
            final[3] = 64;

            Assert.Equal(final, first.Select(x => x * x));
        }

        [Fact]

        public void CheckSelectManyMethode()
        {
            var first = new List<int[]>
            {
                new[]
                {
                    1, 2, 3, 4
                },

                new[]
                {
                    5, 6, 7, 8
                },

                new[]
                {
                    9, 10, 11, 12
                }
            };

            var final = new string[12];
            final[0] = "1";
            final[1] = "2";
            final[2] = "3";
            final[3] = "4";
            final[4] = "5";
            final[5] = "6";
            final[6] = "7";
            final[7] = "8";
            final[8] = "9";
            final[9] = "10";
            final[10] = "11";
            final[11] = "12";

            var result = first.SelectMany(x => x.Select(y => y.ToString()));
            Assert.Equal(final, result);
        }

        [Fact]

        public void CheckWhereMethode()
        {
            var first = new int[4];
            first[0] = 2;
            first[1] = 3;
            first[2] = 4;
            first[3] = 7;

            var final = new int[2];
            final[0] = 2;
            final[1] = 4;
            var result = first.Where(x => x % 2 == 0);
            Assert.Equal(final, result);
        }
    }
}