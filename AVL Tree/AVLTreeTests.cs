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
            Assert.Equal(6, avltree.Root.Key);
            Assert.Equal(7, avltree.Root.RightChild.Key);
            Assert.Equal(4, avltree.Root.LeftChild.Key);
        }

        [Fact]

        public void InsertNodesToAVLTreeParentAreCorrect()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(4);
            avltree.Insert(6);
            avltree.Insert(7);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(6, avltree.Root.RightChild.Parent.Key);
            Assert.Equal(7, avltree.Root.RightChild.Key);
        }

        [Fact]

        public void LLRotationCheck()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(12);
            avltree.Insert(5);
            avltree.Insert(4);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(5, avltree.Root.RightChild.Parent.Key);
            Assert.Equal(12, avltree.Root.RightChild.Key);
            Assert.Equal(4, avltree.Root.LeftChild.Key);
        }

        [Fact]

        public void RRRotationCheck()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(12);
            avltree.Insert(15);
            avltree.Insert(19);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(15, avltree.Root.RightChild.Parent.Key);
            Assert.Equal(19, avltree.Root.RightChild.Key);
            Assert.Equal(12, avltree.Root.LeftChild.Key);
        }

        [Fact]

        public void LRRotationCheck()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(5);
            avltree.Insert(2);
            avltree.Insert(4);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(4, avltree.Root.RightChild.Parent.Key);
            Assert.Equal(5, avltree.Root.RightChild.Key);
            Assert.Equal(2, avltree.Root.LeftChild.Key);
        }

        [Fact]

        public void RLRotationCheck()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(5);
            avltree.Insert(8);
            avltree.Insert(7);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(7, avltree.Root.RightChild.Parent.Key);
            Assert.Equal(8, avltree.Root.RightChild.Key);
            Assert.Equal(5, avltree.Root.LeftChild.Key);
        }
    }
}