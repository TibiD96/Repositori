using System;

namespace BinaryTreeCollection
{
    public class BTreeGraph<T>
    {
        private static BTreeNode<T> node;

        public BTreeGraph()
        {
            node = null;
        }

        public BTreeNode Root => node;

        public int Count { get; set; }

        public int KeysCount { get; set; }

        public void Add(int data)
        {
            BTreeNode newNode = new BTreeNode(data);
            if (node == null)
            {
                node = newNode;
                Count++;
                return;
            }

            actual ??= node;

            BTreeNode parent = actual;

            if (data < actual.Value)
            {
                actual = actual.Left;
                if (actual == null)
                {
                    parent.Left = newNode;
                    Count++;
                    return;
                }
            }
            else
            {
                actual = actual.Right;
                if (actual == null)
                {
                    parent.Right = newNode;
                    Count++;
                    return;
                }
            }

            Add(newNode.Value);
        }

        public int[] Traversel(BTreeNode node, int traversalType)
        {
            int[] traversalResult = new int[Count];
            switch (traversalType)
            {
                case 1:
                    TraverseInOrder(node, traversalResult);
                    break;

                case 2:
                    TraversePreOrder(node, traversalResult);
                    break;

                case 3:
                    TraversePostOrder(node, traversalResult);
                    break;
            }

            return traversalResult;
        }

        private void TraverseInOrder(BTreeNode node, int[] traversalResult)
        {
            if (node == null)
            {
                return;
            }

            TraverseInOrder(node.Left, traversalResult);
            traversalResult[index++] = node.Value;
            TraverseInOrder(node.Right, traversalResult);
        }

        private void TraversePreOrder(BTreeNode node, int[] traversalResult)
        {
            if (node == null)
            {
                return;
            }

            traversalResult[index++] = node.Value;
            TraversePreOrder(node.Left, traversalResult);
            TraversePreOrder(node.Right, traversalResult);
        }

        private void TraversePostOrder(BTreeNode node, int[] traversalResult)
        {
            if (node == null)
            {
                return;
            }

            TraversePostOrder(node.Left, traversalResult);
            TraversePostOrder(node.Right, traversalResult);
            traversalResult[index++] = node.Value;
        }
    }
}
