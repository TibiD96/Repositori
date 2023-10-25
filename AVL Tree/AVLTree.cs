using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryTreeCollection
{
    public class AVLTree<T> where T : IComparable<T>
    {
        internal AVLTreeNode<T> Root;

        public int Count { get; set; }

        public void Insert(T key)
        {
            if (Root == null)
            {
                Root = new AVLTreeNode<T>(key, null);
            }
            else
            {
                if (Contain(key))
                {
                    throw new ArgumentException("Don't allow duplicate values");
                }

                InsertChild(Root, key);
            }

            Count++;
        }

        public void Delete(AVLTreeNode<T> nodeToRemove)
        {
            if (nodeToRemove.Key == null || !Contain(nodeToRemove.Key))
            {
                throw new ArgumentNullException("Node is not present in tree");
            }

            nodeToRemove = FindKeyNode(nodeToRemove.Key);

            if (nodeToRemove.IsLeaf)
            {
              RemoveNode(nodeToRemove);
              Count--;
            }
            else if (nodeToRemove.Left == null || nodeToRemove.Right == null)
            {
              AVLTreeNode<T> replaceWith = nodeToRemove.Left ?? nodeToRemove.Right;
              if (nodeToRemove == Root)
              {
                Root = replaceWith;
              }
              else
              {
                Replace(nodeToRemove, replaceWith);
              }

              Count--;
            }
            else
            {
              AVLTreeNode<T> nextNode = nodeToRemove.Left;
              while (nextNode.Right != null)
              {
                nextNode = nextNode.Right;
              }

              Delete(nextNode);
              nodeToRemove.Key = nextNode.Key;
            }

            Rebalance(nodeToRemove.Parent);
        }

        public AVLTreeNode<T> FindKeyNode(T key)
        {
            AVLTreeNode<T> nodeToCompareWith = Root;
            while (nodeToCompareWith != null)
            {
                int comparison = key.CompareTo(nodeToCompareWith.Key);
                if (comparison == 0)
                {
                    return nodeToCompareWith;
                }
                else if (comparison < 0)
                {
                    nodeToCompareWith = nodeToCompareWith.Left;
                }
                else
                {
                    nodeToCompareWith = nodeToCompareWith.Right;
                }
            }

            return null;
        }

        public void Clear()
        {
            Root = null;
            Count = 0;
        }

        private bool Contain(T key)
        {
            return FindKeyNode(key) != null;
        }

        private int BalanceFactor(AVLTreeNode<T> node)
        {
            return HightOfNode(node.Left) - HightOfNode(node.Right);
        }

        private int HightOfNode(AVLTreeNode<T> node)
        {
            if (node != null)
            {
                return 1 + Math.Max(HightOfNode(node.Left), HightOfNode(node.Right));
            }

            return 0;
        }

        private void InsertChild(AVLTreeNode<T> node, T key)
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

            Rotate(node, balanceFactor);
        }

        private void Rebalance(AVLTreeNode<T> node)
        {
            while (node != null)
            {
                int balanceFactor = BalanceFactor(node);

                if (balanceFactor > 1)
                {
                    if (node.Left.Left != null)
                    {
                        RotateToRight(node);
                    }
                    else
                    {
                        RotateToLeft(node.Left);
                        RotateToRight(node);
                    }
                }
                else if (balanceFactor < -1)
                {
                    if (node.Right.Right != null && node.Right.Left == null)
                    {
                        RotateToLeft(node);
                    }
                    else
                    {
                        RotateToRight(node.Right);
                        RotateToLeft(node);
                    }
                }

                node = node.Parent;
            }
        }

        private void RemoveNode(AVLTreeNode<T> nodeToRemove)
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

        private void Replace(AVLTreeNode<T> nodeToRemove, AVLTreeNode<T> replaceWith)
        {
            replaceWith.Parent = nodeToRemove.Parent;

            if (nodeToRemove == nodeToRemove.Parent.Left)
            {
                nodeToRemove.Parent.Left = replaceWith;
            }
            else
            {
                nodeToRemove.Parent.Right = replaceWith;
            }
        }

        private void Rotate(AVLTreeNode<T> node, int balanceFactor)
        {
            if (balanceFactor > 1)
            {
                if (node.Left.Left != null)
                {
                    RotateToRight(node);
                }
                else
                {
                    RotateToLeft(node.Left);

                    RotateToRight(node);
                }
            }
            else if (balanceFactor < -1)
            {
                if (node.Right.Right != null)
                {
                    RotateToLeft(node);
                }
                else
                {
                    RotateToRight(node.Right);

                    RotateToLeft(node);
                }
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
