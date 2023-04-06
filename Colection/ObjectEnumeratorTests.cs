using Xunit;

namespace CollectionData
{
    public class ObjectEnumeratorTests
    {
        [Fact]

        public void CheckIfConstructorWorks()
        {
            var objEnum = new ObjectArray {123, 'A', 'a', 'B', 'b', "asd"};
            var enumerator = objEnum.GetEnumerator();

            enumerator.MoveNext();
            Assert.Equal(123, enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal('A', enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal('a', enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal('B', enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal('b', enumerator.Current);
            enumerator.MoveNext();
            Assert.Equal("asd", enumerator.Current);
        }
    }
}
