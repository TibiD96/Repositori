using System;

namespace BinaryTreeCollection
{
    public class BTreeGraph<T> where T : IComparable<T>
    {
        private static BTreeNode<T> root;

        private readonly int order;

        public BTreeGraph(int order)
        {
            root = new BTreeNode<T>(order);
            this.order = order;
        }

        public int Count { get; set; }

        public void Add(T key)
        {
            if (root.KeyNumber < order - 1)
            {
                NodeWithFreeSpaces(root, key);
            }
            else
            {
                if (root.IsLeaf)
                {
                    BTreeNode<T> newNode = new BTreeNode<T>(order);
                    newNode.Children[0] = root;
                    DivideChild(newNode, 0, ref key);
                    NodeWithFreeSpaces(newNode, key);
                    root = newNode;
                }
                else
                {
                    NodeWithFreeSpaces(root, key);
                }
            }
        }

        private void NodeWithFreeSpaces(BTreeNode<T> node, T key)
        {
            int indexKeyInNod = 0;
            T temp;

            if (node.KeyNumber == 0)
            {
                node.Keys[indexKeyInNod] = key;
                node.KeyNumber++;
                return;
            }

            if (node.IsLeaf)
            {
                while (key.CompareTo(node.Keys[indexKeyInNod]) >= 0 && indexKeyInNod < node.KeyNumber)
                {
                    indexKeyInNod++;
                }

                if (indexKeyInNod > 0)
                {
                    node.Keys[indexKeyInNod] = key;
                    node.KeyNumber++;
                    return;
                }

                temp = node.Keys[indexKeyInNod];
                node.Keys[indexKeyInNod + 1] = temp;
                node.Keys[indexKeyInNod] = key;
                node.KeyNumber++;
            }
            else
            {
                while (key.CompareTo(node.Keys[indexKeyInNod]) >= 0 && indexKeyInNod <= node.KeyNumber)
                {
                    indexKeyInNod++;
                    if (indexKeyInNod == node.KeyNumber)
                    {
                        break;
                    }
                }

                BTreeNode<T> newnode = node.Children[indexKeyInNod];
                if (newnode.KeyNumber == order - 1)
                {
                    DivideChild(node, indexKeyInNod, ref key);
                    node.Keys[indexKeyInNod] = key;
                    node.KeyNumber++;
                    return;
                }

                NodeWithFreeSpaces(node.Children[indexKeyInNod], key);
            }
        }

        private void DivideChild(BTreeNode<T> node, int indexOfNodeToSplit, ref T key)
        {
            BTreeNode<T> nodeToSplit = node.Children[indexOfNodeToSplit];
            T keyNewValue = key;

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
                node.Children[indexOfNodeToSplit] = new BTreeNode<T>(order);
                node.Children[indexOfNodeToSplit].Keys[0] = nodeToSplit.Keys[j];
                node.Children[indexOfNodeToSplit].KeyNumber++;
                indexOfNodeToSplit++;
            }
        }
    }
}
