using Xunit;

namespace BinaryTreeCollection
{
    public class BTreeTests
    {
        [Fact]

        public void AddNodesToAVLTree()
        {
            AVLTree<int> avltree = new AVLTree<int>();
            avltree.Insert(4);
            avltree.Insert(6);
            avltree.Insert(7);
        }
    }
}