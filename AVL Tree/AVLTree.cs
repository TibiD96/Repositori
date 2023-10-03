using System;
using System.Collections.Generic;
using System.Text;

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
            int balanceFactor;
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

            balanceFactor = BalanceFactor(node);

            if (balanceFactor >= -1 && balanceFactor <= 1)
            {
                return;
            }

            //Rotate(node, balanceFactor);
        }

        public int BalanceFactor(AVLTreeNode<T> node)
        {
            return HightOfNode(node.LeftChild) - HightOfNode(node.RightChild);
        }

        public int HightOfNode(AVLTreeNode<T> node)
        {
            if (node != null)
            {
                return 1 + Math.Max(HightOfNode(node.LeftChild), HightOfNode(node.RightChild));
            }

            return 0;
        }

        //private void Rotate(AVLTreeNode<T> node, int balanceFactor)
        //{

        //}
    }
}
