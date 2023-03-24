using Xunit;

namespace CollectionData
{
    public class IntArrayTests
    {
        [Fact]

        public void AddElemetsInArrayPositioningItOnTheEnd()
        {
            var input = new IntArray();
            input.Add(5);
            input.Add(10);
            input.Add(100);
            input.Add(25);
            Assert.Equal(4, input.Count());
            Assert.Equal(25, input.Element(input.Count() - 1));
        }

        [Fact]

        public void ChangeValueOfTheElementFromGivenIndex()
        {
            var input = new IntArray();
            int correctValueAfterChanging = 1000;
            input.Add(5);
            input.Add(10);
            input.Add(100);
            input.Add(25);
            input.SetElement(0, 1000);
            Assert.Equal(input.Element(0), correctValueAfterChanging);
        }
    }
}