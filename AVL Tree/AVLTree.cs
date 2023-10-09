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

            Rotate(ref node, ref balanceFactor, key);
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

        private void Rotate(ref AVLTreeNode<T> node, ref int balanceFactor, T key)
        {
            if (balanceFactor > 1 && key.CompareTo(node.LeftChild.Key) < 0)
            {
                RotateToRight(node);

                balanceFactor = BalanceFactor(node);
            }

            if (balanceFactor > 1 && key.CompareTo(node.LeftChild.Key) > 0)
            {
                RotateToLeft(node.LeftChild);

                RotateToRight(node);

                balanceFactor = BalanceFactor(node);
            }

            if (balanceFactor < -1 && key.CompareTo(node.RightChild.Key) > 0)
            {
                RotateToLeft(node);

                balanceFactor = BalanceFactor(node);
            }

            if (balanceFactor < -1 && key.CompareTo(node.RightChild.Key) < 0)
            {
                RotateToRight(node.RightChild);

                RotateToLeft(node);

                balanceFactor = BalanceFactor(node);
            }
        }

        private void RotateToRight(AVLTreeNode<T> node)
        {
            AVLTreeNode<T> pivot = node.LeftChild;
            node.LeftChild = pivot.RightChild;

            if (pivot.RightChild != null)
            {
                pivot.RightChild.Parent = node;
            }

            pivot.Parent = node.Parent;

            if (node.Parent == null)
            {
                Root = pivot;
            }
            else if (node == node.Parent.LeftChild)
            {
                node.Parent.LeftChild = pivot;
            }
            else
            {
                node.Parent.RightChild = pivot;
            }

            pivot.RightChild = node;
            node.Parent = pivot;
        }

        private void RotateToLeft(AVLTreeNode<T> node)
        {
            AVLTreeNode<T> pivot = node.RightChild;
            node.RightChild = pivot.LeftChild;

            if (pivot.LeftChild != null)
            {
                pivot.LeftChild.Parent = node;
            }

            pivot.Parent = node.Parent;

            if (node.Parent == null)
            {
                Root = pivot;
            }
            else if (node == node.Parent.LeftChild)
            {
                node.Parent.LeftChild = pivot;
            }
            else
            {
                node.Parent.RightChild = pivot;
            }

            pivot.LeftChild = node;
            node.Parent = pivot;
        }
    }
}
