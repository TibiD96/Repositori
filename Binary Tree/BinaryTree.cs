using System;

namespace BinaryTreeCollection
{
    public class BinaryTreeGraph
    {
        private static Node node;

        private int[] traversalResult;

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

        public int[] Traversal(Node node)
        {
            int index = 0;
            traversalResult = new int[Count];
            TraverseInOrder(node, ref index);
            return traversalResult;
        }

        private void TraverseInOrder(Node node, ref int index)
        {
            if (node == null)
            {
                return;
            }

            TraverseInOrder(node.Left, ref index);
            traversalResult[index] = node.Value;
            index++;
            TraverseInOrder(node.Right, ref index);
        }
    }
}
