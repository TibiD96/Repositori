using Xunit;

namespace BinaryTreeCollection
{
    public class BTreeTests
    {
        [Fact]

        public void InsertNodesToAVLTree()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(4);
            avltree.Insert(6);
            avltree.Insert(7);
            Assert.Equal(4, avltree.Root.Key);
            Assert.Equal(6, avltree.Root.RightChild.Key);
            Assert.Equal(7, avltree.Root.RightChild.RightChild.Key);
        }

        [Fact]

        public void InsertNodesToAVLTreeParentAreCorrect()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(4);
            avltree.Insert(6);
            avltree.Insert(7);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(4, avltree.Root.RightChild.Parent.Key);
            Assert.Equal(6, avltree.Root.RightChild.RightChild.Parent.Key);
        }
    }
}