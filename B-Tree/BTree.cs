using System;

namespace BinaryTreeCollection
{
    public class BTreeGraph<T> where T : IComparable<T>
    {
        private static BTreeNode<T> root;

        private readonly int order;

        public BTreeGraph(int order)
        {
            root = null;
            this.order = order;
        }

        public int Count { get; set; }

        public void Add(T key)
        {
            if (root == null)
            {
                root = new BTreeNode<T>(order);
                root.Keys[0] = key;
                root.KeyNumber = 1;
            }
            else
            {
                if (root.KeyNumber < order - 1)
                {
                    NodeWithFreeSpaces(root, key);
                }
                else
                {
                    BTreeNode<T> newNode = new BTreeNode<T>(order);
                    newNode.Children[0] = root;
                    DivideChild(newNode, 0, ref key);
                    NodeWithFreeSpaces(newNode, key);
                    root = newNode;
                }
            }
        }

        private void NodeWithFreeSpaces(BTreeNode<T> node, T key)
        {
            int indexKeyInNod = 0;
            T temp;

            while (key.CompareTo(node.Keys[indexKeyInNod]) >= 0 && indexKeyInNod < node.KeyNumber)
            {
                indexKeyInNod++;
            }

            if (node.IsLeaf || node.KeyNumber == 0)
            {
                if (indexKeyInNod > 0)
                {
                    node.Keys[indexKeyInNod] = key;
                    node.KeyNumber++;
                }
                else
                {
                    temp = node.Keys[indexKeyInNod];
                    node.Keys[indexKeyInNod + 1] = temp;
                    node.Keys[indexKeyInNod] = key;
                    node.KeyNumber++;
                }
            }
            else
            {
                BTreeNode<T> newnode = node.Children[indexKeyInNod];
                if (newnode.KeyNumber == order - 1)
                {
                    DivideChild(node, indexKeyInNod, ref key);
                    if (key.CompareTo(node.Keys[indexKeyInNod]) > 0)
                    {
                        indexKeyInNod++;
                    }
                }

                NodeWithFreeSpaces(node.Children[indexKeyInNod], key);
            }
        }

        private void DivideChild(BTreeNode<T> node, int indexOfNodeToSplit, ref T key)
        {
            BTreeNode<T> nodeToSplit = node.Children[indexOfNodeToSplit];
            T keyNewValue = key;
            for (int childNumber = 0; !nodeToSplit.IsLeaf; childNumber++)
            {
                if (key.CompareTo(nodeToSplit.Keys[childNumber]) < 0 && childNumber == 0)
                {
                    nodeToSplit = nodeToSplit.Children[0];
                    break;
                }

                if (key.CompareTo(nodeToSplit.Keys[childNumber]) > 0 && key.CompareTo(nodeToSplit.Keys[childNumber + 1]) < 0)
                {
                    nodeToSplit = nodeToSplit.Children[1];
                    break;
                }

                nodeToSplit = nodeToSplit.Children[order - 1];
            }

            for (int i = 0; i < nodeToSplit.Keys.Length; i++)
            {
                if (key.CompareTo(nodeToSplit.Keys[i]) < 0 && i == 0)
                {
                    T temp = nodeToSplit.Keys[i];
                    nodeToSplit.Keys[i] = key;
                    keyNewValue = temp;
                }
                else if (key.CompareTo(nodeToSplit.Keys[i]) > 0 && i > 0)
                {
                    T temp = nodeToSplit.Keys[i];
                    nodeToSplit.Keys[i] = key;
                    keyNewValue = temp;
                }
            }

            key = keyNewValue;
            for (int j = 0; j < nodeToSplit.Keys.Length; j++)
            {
                node.Children[j] = new BTreeNode<T>(order);
                NodeWithFreeSpaces(node.Children[j], nodeToSplit.Keys[j]);
            }
        }
    }
}
