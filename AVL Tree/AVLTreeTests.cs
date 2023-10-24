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
            Assert.Equal(7, avltree.Root.Right.Key);
            Assert.Equal(4, avltree.Root.Left.Key);
            Assert.Equal(3, avltree.Count);
        }

        [Fact]

        public void InsertNodesToAVLTreeParentAreCorrect()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(4);
            avltree.Insert(6);
            avltree.Insert(7);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(6, avltree.Root.Right.Parent.Key);
            Assert.Equal(7, avltree.Root.Right.Key);
            Assert.Equal(3, avltree.Count);
        }

        [Fact]

        public void LLRotationCheck()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(12);
            avltree.Insert(5);
            avltree.Insert(4);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(5, avltree.Root.Right.Parent.Key);
            Assert.Equal(12, avltree.Root.Right.Key);
            Assert.Equal(4, avltree.Root.Left.Key);
            Assert.Equal(3, avltree.Count);
        }

        [Fact]

        public void RRRotationCheck()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(12);
            avltree.Insert(15);
            avltree.Insert(19);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(15, avltree.Root.Right.Parent.Key);
            Assert.Equal(19, avltree.Root.Right.Key);
            Assert.Equal(12, avltree.Root.Left.Key);
            Assert.Equal(3, avltree.Count);
        }

        [Fact]

        public void LRRotationCheck()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(5);
            avltree.Insert(2);
            avltree.Insert(4);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(4, avltree.Root.Right.Parent.Key);
            Assert.Equal(5, avltree.Root.Right.Key);
            Assert.Equal(2, avltree.Root.Left.Key);
            Assert.Equal(3, avltree.Count);
        }

        [Fact]

        public void RLRotationCheck()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(5);
            avltree.Insert(8);
            avltree.Insert(7);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(7, avltree.Root.Right.Parent.Key);
            Assert.Equal(8, avltree.Root.Right.Key);
            Assert.Equal(5, avltree.Root.Left.Key);
            Assert.Equal(3, avltree.Count);
        }

        [Fact]

        public void MoreComplexTreeInsertAndRebalance()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(1);
            avltree.Insert(2);
            avltree.Insert(3);
            avltree.Insert(4);
            avltree.Insert(5);
            avltree.Insert(6);
            avltree.Insert(7);
            Assert.Null(avltree.Root.Parent);
            Assert.Equal(4, avltree.Root.Key);
            Assert.Equal(6, avltree.Root.Right.Key);
            Assert.Equal(2, avltree.Root.Left.Key);
            Assert.Equal(1, avltree.Root.Left.Left.Key);
            Assert.Equal(3, avltree.Root.Left.Right.Key);
            Assert.Equal(7, avltree.Root.Right.Right.Key);
            Assert.Equal(5, avltree.Root.Right.Left.Key);
            Assert.Equal(7, avltree.Count);
        }

        [Fact]

        public void FindMethodeShouldReturnTrue()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(1);
            avltree.Insert(2);
            avltree.Insert(3);
            avltree.Insert(4);
            avltree.Insert(5);
            avltree.Insert(6);
            avltree.Insert(7);
            AVLTree<int> compareWith = new AVLTree<int>();
            compareWith.Insert(6);
            compareWith.Insert(5);
            compareWith.Insert(7);
            Assert.Equal(avltree.FindKeyNode(compareWith.Root.Key).Key, avltree.Root.Right.Key);
            Assert.Equal(7, avltree.Count);
        }

        [Fact]

        public void FindMethodeShouldReturnFalse()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(1);
            avltree.Insert(2);
            avltree.Insert(3);
            avltree.Insert(4);
            avltree.Insert(5);
            avltree.Insert(6);
            avltree.Insert(7);
            AVLTree<int> compareWith = new AVLTree<int>();
            compareWith.Insert(5);
            compareWith.Insert(10);
            compareWith.Insert(15);
            Assert.Null(avltree.FindKeyNode(compareWith.Root.Key));
            Assert.Equal(7, avltree.Count);
        }

        [Fact]

        public void CheckDeleteForANodeWhichIsLeaf()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(1);
            avltree.Insert(2);
            avltree.Insert(3);
            AVLTree<int> deleteNode = new AVLTree<int>();
            deleteNode.Insert(3);
            avltree.Delete(deleteNode.Root);
            Assert.Null(avltree.Root.Right);
            deleteNode.Insert(1);
            avltree.Delete(deleteNode.Root.Left);
            Assert.Null(avltree.Root.Left);
            Assert.Equal(1, avltree.Count);
        }

        [Fact]

        public void CheckDeleteForANodeWithOneChildNoRebalanceNeeded()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(4);
            avltree.Insert(3);
            avltree.Insert(2);
            avltree.Insert(1);
            AVLTree<int> deleteNode = new AVLTree<int>();
            deleteNode.Insert(2);
            avltree.Delete(deleteNode.Root);
            Assert.Equal(1, avltree.Root.Left.Key);
            Assert.Equal(3, avltree.Root.Left.Parent.Key);
            Assert.Equal(3, avltree.Count);
        }

        [Fact]

        public void CheckDeleteForANodeWithTwoChildWhichAreLeafsNoRebalanceNeeded()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(20);
            avltree.Insert(25);
            avltree.Insert(30);
            avltree.Insert(35);
            avltree.Insert(40);
            avltree.Insert(45);
            avltree.Insert(50);
            AVLTree<int> deleteNode = new AVLTree<int>();
            deleteNode.Insert(45);
            avltree.Delete(deleteNode.Root);
            Assert.Equal(40, avltree.Root.Right.Key);
            Assert.Equal(40, avltree.Root.Right.Right.Parent.Key);
            Assert.Equal(6, avltree.Count);
        }

        [Fact]

        public void CheckDeleteForANodeWithTwoChildNoRebalanceNeeded()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(20);
            avltree.Insert(25);
            avltree.Insert(30);
            avltree.Insert(35);
            avltree.Insert(40);
            avltree.Insert(45);
            avltree.Insert(50);
            avltree.Insert(43);
            AVLTree<int> deleteNode = new AVLTree<int>();
            deleteNode.Insert(45);
            avltree.Delete(deleteNode.Root);
            Assert.Equal(43, avltree.Root.Right.Key);
            Assert.Equal(43, avltree.Root.Right.Right.Parent.Key);
            Assert.Equal(40, avltree.Root.Right.Left.Key);
            Assert.Equal(7, avltree.Count);
        }

        [Fact]

        public void CheckRebalanceAfterDeleteFirstTest()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(20);
            avltree.Insert(25);
            avltree.Insert(30);
            avltree.Insert(35);
            avltree.Insert(40);
            avltree.Insert(45);
            avltree.Insert(50);
            avltree.Insert(43);
            AVLTree<int> deleteNode1 = new AVLTree<int>();
            deleteNode1.Insert(20);
            avltree.Delete(deleteNode1.Root);
            AVLTree<int> deleteNode2 = new AVLTree<int>();
            deleteNode2.Insert(30);
            avltree.Delete(deleteNode2.Root);
            Assert.Equal(45, avltree.Root.Right.Key);
            Assert.Equal(45, avltree.Root.Right.Right.Parent.Key);
            Assert.Equal(43, avltree.Root.Right.Left.Key);
            Assert.Equal(6, avltree.Count);
        }

        [Fact]

        public void CheckRebalanceAfterDeleteSecondTest()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(10);
            avltree.Insert(5);
            avltree.Insert(15);
            avltree.Insert(3);
            avltree.Insert(7);
            avltree.Insert(12);
            avltree.Insert(20);
            avltree.Insert(2);
            AVLTree<int> deleteNode1 = new AVLTree<int>();
            deleteNode1.Insert(12);
            avltree.Delete(deleteNode1.Root);
            AVLTree<int> deleteNode2 = new AVLTree<int>();
            deleteNode2.Insert(20);
            avltree.Delete(deleteNode2.Root);
            Assert.Equal(10, avltree.Root.Right.Key);
            Assert.Equal(10, avltree.Root.Right.Right.Parent.Key);
            Assert.Equal(7, avltree.Root.Right.Left.Key);
            Assert.Equal(6, avltree.Count);
        }

        [Fact]

        public void DeletingRootNode()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(1);
            avltree.Insert(2);
            avltree.Insert(3);
            avltree.Insert(4);
            avltree.Insert(5);
            avltree.Insert(6);
            avltree.Insert(7);
            AVLTree<int> deleteNode1 = new AVLTree<int>();
            deleteNode1.Insert(4);
            avltree.Delete(deleteNode1.Root);
            Assert.Equal(3, avltree.Root.Key);
            Assert.Equal(6, avltree.Count);
        }

        [Fact]

        public void ThrowArgumentForDuplciateValues()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(1);
            avltree.Insert(2);
            avltree.Insert(3);
            Assert.Throws<ArgumentException>(() => avltree.Insert(3));
            Assert.Equal(3, avltree.Count);
        }

        [Fact]

        public void ThrowArgumentForInexistentKeyInTree()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(1);
            avltree.Insert(2);
            avltree.Insert(3);
            AVLTree<int> deleteNode1 = new AVLTree<int>();
            deleteNode1.Insert(4);
            Assert.Throws<ArgumentNullException>(() => avltree.Delete(deleteNode1.Root));
            Assert.Equal(3, avltree.Count);
        }
    }
}