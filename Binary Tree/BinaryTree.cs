using System;

namespace BinaryTreeCollection
{
    public class BinaryTreeGraph
    {
        private static Node node;

        private Node actual;

        public BinaryTreeGraph()
        {
            node = null;
        }

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

            if (data <= actual.Value)
            {
                actual = actual.Left;
                if (actual == null)
                {
                    parent.Left = newNode;
                    Count++;
                }
                else
                {
                    Add(newNode.Value);
                }
            }
            else
            {
                actual = actual.Right;
                if (actual == null)
                {
                    parent.Right = newNode;
                    Count++;
                }
                else
                {
                    Add(newNode.Value);
                }
            }
        }
    }
}
