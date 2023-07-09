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

        [Fact]
        public void CheckIfTraversalWorks()
        {
            BinaryTreeGraph binaryTree = new BinaryTreeGraph();
            binaryTree.Add(5);
            binaryTree.Add(8);
            binaryTree.Add(3);
            binaryTree.Add(10);
            binaryTree.Add(7);
            binaryTree.Add(4);
            binaryTree.Add(2);

            int[] expectedTraversal = new[] { 2, 3, 4, 5, 7, 8, 10 };

            int[] actualTraversal = binaryTree.Traversal(binaryTree.Root);

            Assert.Equal(expectedTraversal, actualTraversal);
        }
    }
}
