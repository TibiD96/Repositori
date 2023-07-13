using Xunit;

namespace BinaryTreeCollection
{
    public class BTreeTests
    {
        [Fact]

        public void AddNodesToBTree()
        {
            BTreeGraph<int> btree = new BTreeGraph<int>(3);
            btree.Add(2);
            btree.Add(4);
            btree.Add(3);
        }
    }
}
