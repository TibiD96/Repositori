using Xunit;

namespace CollectionData
{
    public class ObjectEnumeratorTests
    {
        [Fact]

        public void CheckIfConstructorWorks()
        {
            var objEnum = new ObjectArray();
            objEnum.Add("qwerty");
            objEnum.Add('a');
            objEnum.Add('A');
            objEnum.Add(123);
            objEnum.Add(12.3);
            Assert.Equal("qwerty", objEnum[0]);
            Assert.Equal('a', objEnum[1]);
            Assert.Equal('A', objEnum[2]);
            Assert.Equal(123, objEnum[3]);
            Assert.Equal(12.3, objEnum[4]);
        }
    }
}
