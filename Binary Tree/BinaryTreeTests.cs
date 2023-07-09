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
        public void CheckIfTraversalInOrderWorks()
        {
            BinaryTreeGraph binaryTree = new BinaryTreeGraph();
            binaryTree.Add(5);
            binaryTree.Add(8);
            binaryTree.Add(3);
            binaryTree.Add(10);
            binaryTree.Add(7);
            binaryTree.Add(4);
            binaryTree.Add(2);

            const int traversalType = 1;
            int[] expectedTraversal = new[] { 2, 3, 4, 5, 7, 8, 10 };
            int[] actualTraversal = binaryTree.Traversel(binaryTree.Root, traversalType);
            Assert.Equal(expectedTraversal, actualTraversal);
        }

        [Fact]
        public void CheckIfTraversalPreOrderWorks()
        {
            BinaryTreeGraph binaryTree = new BinaryTreeGraph();
            binaryTree.Add(5);
            binaryTree.Add(8);
            binaryTree.Add(3);
            binaryTree.Add(10);
            binaryTree.Add(7);
            binaryTree.Add(4);
            binaryTree.Add(2);

            const int traversalType = 2;
            int[] expectedTraversal = new[] { 5, 3, 2, 4, 8, 7, 10 };
            int[] actualTraversal = binaryTree.Traversel(binaryTree.Root, traversalType);
            Assert.Equal(expectedTraversal, actualTraversal);
        }

        [Fact]
        public void CheckIfTraversalPostOrderWorks()
        {
            BinaryTreeGraph binaryTree = new BinaryTreeGraph();
            binaryTree.Add(5);
            binaryTree.Add(8);
            binaryTree.Add(3);
            binaryTree.Add(10);
            binaryTree.Add(7);
            binaryTree.Add(4);
            binaryTree.Add(2);

            const int traversalType = 3;
            int[] expectedTraversal = new[] { 2, 4, 3, 7, 10, 8, 5 };
            int[] actualTraversal = binaryTree.Traversel(binaryTree.Root, traversalType);
            Assert.Equal(expectedTraversal, actualTraversal);
        }
    }
}
