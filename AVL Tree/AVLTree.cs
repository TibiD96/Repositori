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
                if (node.Left == null)
                {
                    node.Left = new AVLTreeNode<T>(key, node);
                }
                else
                {
                    InsertChild(node.Left, key);
                }
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new AVLTreeNode<T>(key, node);
                }
                else
                {
                    InsertChild(node.Right, key);
                }
            }

            balanceFactor = BalanceFactor(node);

            if (balanceFactor >= -1 && balanceFactor <= 1)
            {
                return;
            }

            Rotate(node, ref balanceFactor, key);
        }

        public void Delete(AVLTreeNode<T> nodeToRemove)
        {
            nodeToRemove = FindNode(nodeToRemove);
            if (nodeToRemove != null)
            {
                if (nodeToRemove.IsLeaf)
                {
                    RemoveNode(nodeToRemove);
                }
                else if (nodeToRemove.Left == null || nodeToRemove.Right == null)
                {

                }
            }
        }

        public void RemoveNode(AVLTreeNode<T> nodeToRemove)
        {
            if (nodeToRemove.Parent == null)
            {
                Root = null;
            }
            else if (nodeToRemove == nodeToRemove.Parent.Left)
            {
                nodeToRemove.Parent.Left = null;
            }
            else
            {
                nodeToRemove.Parent.Right = null;
            }
        }

        public AVLTreeNode<T> FindNode(AVLTreeNode<T> nodeToFind)
        {
            AVLTreeNode<T> nodeToComapreWith = Root;
            while (nodeToFind.Key.CompareTo(nodeToComapreWith.Key) < 0 || nodeToFind.Key.CompareTo(nodeToComapreWith.Key) > 0)
            {
                nodeToComapreWith = nodeToFind.Key.CompareTo(nodeToComapreWith.Key) < 0 ? nodeToComapreWith.Left : nodeToComapreWith.Right;

                if (nodeToComapreWith == null)
                {
                    return null;
                }
            }

            return nodeToComapreWith;
        }

        public int BalanceFactor(AVLTreeNode<T> node)
        {
            return HightOfNode(node.Left) - HightOfNode(node.Right);
        }

        public int HightOfNode(AVLTreeNode<T> node)
        {
            if (node != null)
            {
                return 1 + Math.Max(HightOfNode(node.Left), HightOfNode(node.Right));
            }

            return 0;
        }

        private void Rotate(AVLTreeNode<T> node, ref int balanceFactor, T key)
        {
            if (balanceFactor > 1 && key.CompareTo(node.Left.Key) < 0)
            {
                RotateToRight(node);

                balanceFactor = BalanceFactor(node);
            }

            if (balanceFactor > 1 && key.CompareTo(node.Left.Key) > 0)
            {
                RotateToLeft(node.Left);

                RotateToRight(node);

                balanceFactor = BalanceFactor(node);
            }

            if (balanceFactor < -1 && key.CompareTo(node.Right.Key) > 0)
            {
                RotateToLeft(node);

                balanceFactor = BalanceFactor(node);
            }

            if (balanceFactor < -1 && key.CompareTo(node.Right.Key) < 0)
            {
                RotateToRight(node.Right);

                RotateToLeft(node);

                balanceFactor = BalanceFactor(node);
            }
        }

        private void RotateToRight(AVLTreeNode<T> node)
        {
            AVLTreeNode<T> pivot = node.Left;
            node.Left = pivot.Right;

            if (pivot.Right != null)
            {
                pivot.Right.Parent = node;
            }

            pivot.Parent = node.Parent;

            if (node.Parent == null)
            {
                Root = pivot;
            }
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = pivot;
            }
            else
            {
                node.Parent.Right = pivot;
            }

            pivot.Right = node;
            node.Parent = pivot;
        }

        private void RotateToLeft(AVLTreeNode<T> node)
        {
            AVLTreeNode<T> pivot = node.Right;
            node.Right = pivot.Left;

            if (pivot.Left != null)
            {
                pivot.Left.Parent = node;
            }

            pivot.Parent = node.Parent;

            if (node.Parent == null)
            {
                Root = pivot;
            }
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = pivot;
            }
            else
            {
                node.Parent.Right = pivot;
            }

            pivot.Left = node;
            node.Parent = pivot;
        }
    }
}
