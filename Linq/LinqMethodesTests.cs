using System.Collections.Generic;
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
        }
    }
}