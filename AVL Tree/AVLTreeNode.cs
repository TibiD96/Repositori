using System.ComponentModel;

namespace BinaryTreeCollection
{
    public class AVLTreeNode<T>
    {
        public AVLTreeNode<T> Left;
        public AVLTreeNode<T> Right;
        public T Key;
        public AVLTreeNode<T> Parent;

        public AVLTreeNode(T key, AVLTreeNode<T> parent)
        {
            this.Key = key;
            this.Parent = parent;
        }

        public bool IsLeaf
        {
            get { return Left == null && Right == null; }
        }
    }
}