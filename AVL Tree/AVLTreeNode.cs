using System.ComponentModel;

namespace BinaryTreeCollection
{
    public class AVLTreeNode<T>
    {
        public AVLTreeNode<T> LeftChild;
        public AVLTreeNode<T> RightChild;
        public T Key;
        public AVLTreeNode<T> Parent;

        public AVLTreeNode(T key, AVLTreeNode<T> parent)
        {
            this.Key = key;
            this.Parent = parent;
        }
    }
}