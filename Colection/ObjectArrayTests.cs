using Xunit;

namespace CollectionData
{
    public class ObjectArrayTests
    {
        [Fact]

        public void CheckIfConstructorWorks()
        {
            var obj = new ObjectArray();
            Assert.Equal(0, obj.Count);
        }

        [Fact]

        public void CheckIfAddMethodeWorks()
        {
            var obj = new ObjectArray();
            obj.Add("qwerty");
            obj.Add('a');
            obj.Add('A');
            obj.Add(123);
            obj.Add(12.3);
            Assert.Equal("qwerty", obj[0]);
            Assert.Equal('a', obj[1]);
            Assert.Equal('A', obj[2]);
            Assert.Equal(123, obj[3]);
            Assert.Equal(12.3, obj[4]);
        }

        [Fact]

        public void ReturnTrueIfElementIsContained()
        {
            var obj = new ObjectArray();
            string elementToCheck = "qwerty";
            obj.Add("qwerty");
            obj.Add('a');
            obj.Add('A');
            obj.Add(123);
            obj.Add(12.3);
            Assert.True(obj.Contains(elementToCheck));
        }
    }
}