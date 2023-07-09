using System;

namespace BinaryTreeCollection
{
    public class BinaryTreeGraph
    {
        private static Node node;

        private int index;

        private Node actual;

        public BinaryTreeGraph()
        {
            node = null;
        }

        public Node Root => node;

        public int Count { get; set; }

        public void Add(int data)
        {
            Node newNode = new Node(data);
            if (node == null)
            {
                node = newNode;
                Count++;
                return;
            }

            actual ??= node;

            Node parent = actual;

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

        public int[] Traversel(Node node, int traversalType)
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

        private void TraverseInOrder(Node node, int[] traversalResult)
        {
            if (node == null)
            {
                return;
            }

            TraverseInOrder(node.Left, traversalResult);
            traversalResult[index++] = node.Value;
            TraverseInOrder(node.Right, traversalResult);
        }

        private void TraversePreOrder(Node node, int[] traversalResult)
        {
            if (node == null)
            {
                return;
            }

            traversalResult[index++] = node.Value;
            TraversePreOrder(node.Left, traversalResult);
            TraversePreOrder(node.Right, traversalResult);
        }

        private void TraversePostOrder(Node node, int[] traversalResult)
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
