using Xunit;

namespace BinaryTreeCollection
{
    public class BinaryTreeTests
    {
        [Fact]

        public void AddNodesToBinaryTree()
        {
            BinaryTreeGraph binaryTree = new BinaryTreeGraph();
            binaryTree.Add(5);
            binaryTree.Add(8);
            binaryTree.Add(3);
            binaryTree.Add(9);
            Assert.Equal(4, binaryTree.Count);
        }
    }
}
