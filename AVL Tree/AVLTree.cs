using System;
using System.Security.Cryptography;

namespace BinaryTreeCollection
{
    public class AVLTree<T> where T : IComparable<T>
    {
        public AVLTreeNode<T> Root;

        public int Count { get; set; }

        public void Insert(T key)
        {
            if (Root == null)
            {
                Root = new AVLTreeNode<T>(key, null);
            }
            else
            {
                InsertChild(Root, key);
            }

            Count++;
        }

        public void InsertChild(AVLTreeNode<T> node, T key)
        {
            if (key.CompareTo(node.Key) < 0)
            {
                if (node.LeftChild == null)
                {
                    node.LeftChild = new AVLTreeNode<T>(key, node);
                }
                else
                {
                    InsertChild(node.LeftChild, key);
                }
            }
            else
            {
                if (node.RightChild == null)
                {
                    node.RightChild = new AVLTreeNode<T>(key, node);
                }
                else
                {
                    InsertChild(node.RightChild, key);
                }
            }
        }
    }
}
