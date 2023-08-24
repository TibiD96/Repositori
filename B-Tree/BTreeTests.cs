using Xunit;

namespace BinaryTreeCollection
{
    public class BTreeTests
    {
        [Fact]

        public void AddNodesToBTree()
        {
            BTreeGraph<int> btree = new BTreeGraph<int>(3);
            btree.Add(1);
            btree.Add(2);
            btree.Add(3);
            btree.Add(4);
            btree.Add(5);
            btree.Add(6);
            btree.Add(7);
        }

        [Fact]

        public void AddNodesToBTreeSecondTest()
        {
            BTreeGraph<int> btree = new BTreeGraph<int>(3);
            btree.Add(3);
            btree.Add(8);
            btree.Add(1);
        }
    }
}