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

        [Fact]

        public void ReturnFalseIfElementIsContained()
        {
            var obj = new ObjectArray();
            string elementToCheck = "zxc";
            obj.Add("qwerty");
            obj.Add('a');
            obj.Add('A');
            obj.Add(123);
            obj.Add(12.3);
            Assert.False(obj.Contains(elementToCheck));
        }

        [Fact]

        public void ReturnTheIndexOfGivenNumberElseReturnMinusOne()
        {
            var obj = new ObjectArray();
            obj.Add("qwerty");
            obj.Add('a');
            obj.Add('A');
            obj.Add(123);
            obj.Add(12.3);
            Assert.Equal(2, obj.IndexOf('A'));
            Assert.Equal(-1, obj.IndexOf(25.5));
        }

        [Fact]

        public void InsertTheNumberToTheSpecifiedPosition()
        {
            var obj = new ObjectArray();
            obj.Add("qwerty");
            obj.Add('a');
            obj.Add('A');
            obj.Add(123);
            obj.Add(12.3);
            obj.Insert(2, 3);
            Assert.Equal('B', obj[2]);
            Assert.Equal('A', obj[3]);
        }

        [Fact]

        public void DeleteAllElementsFfromArray()
        {
            var obj = new ObjectArray();
            obj.Add("qwerty");
            obj.Add('a');
            obj.Add('A');
            obj.Add(123);
            obj.Add(12.3);
            obj.Clear();
            Assert.Equal(0, obj.Count);
        }
    }
}