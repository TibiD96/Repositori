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
            if (root.IsLeaf)
            {
                if (root.KeyNumber < order - 1)
                {
                    root.Keys[root.KeyNumber] = key;
                    root.KeyNumber++;
                }
                else
                {
                    BTreeNode<T> newNode = new BTreeNode<T>(order);
                    newNode.Children[0] = root;
                    DivideChild(ref newNode, 0, key);
                    root = newNode;
                }
            }
            else
            {
                NodeWithFreeSpaces(ref root, key);
            }
        }

        private void NodeWithFreeSpaces(ref BTreeNode<T> node, T key)
        {
            int indexKeyInNod = 0;
            T temp;

            while (key.CompareTo(node.Keys[indexKeyInNod]) >= 0)
            {
                indexKeyInNod++;
                if (indexKeyInNod == node.KeyNumber)
                {
                    break;
                }
            }

            BTreeNode<T> newnode = node.Children[indexKeyInNod];

            if (newnode.IsLeaf)
            {
                int indexOfNewKey = newnode.KeyNumber;
                if (newnode.KeyNumber == order - 1)
                {
                    DivideChild(ref node, indexKeyInNod, key);
                }
                else
                {
                    if (indexKeyInNod > 0)
                    {
                        newnode.Keys[indexOfNewKey] = key;
                        node.Children[indexKeyInNod] = newnode;
                        node.Children[indexKeyInNod].KeyNumber++;
                    }
                    else
                    {
                        temp = newnode.Keys[newnode.KeyNumber];
                        newnode.Keys[newnode.KeyNumber + 1] = temp;
                        newnode.Keys[newnode.KeyNumber] = key;
                        node.Children[newnode.KeyNumber] = newnode;
                        node.Children[indexKeyInNod].KeyNumber++;
                    }
                }
            }
            else
            {
                NodeWithFreeSpaces(ref node.Children[indexKeyInNod], key);
            }
        }

        private void DivideChild(ref BTreeNode<T> node, int indexOfNodeToSplit, T key)
        {
            BTreeNode<T> parentNode = node;
            BTreeNode<T> nodeToSplit = node.Children[indexOfNodeToSplit];
            T keyNewValue = key;

            if (parentNode.KeyNumber == order - 1)
            {
                BTreeNode<T> newNode = new BTreeNode<T>(order);
                newNode.Children[0] = nodeToSplit;
                DivideChild(ref newNode, 0, keyNewValue);
                parentNode.Children[indexOfNodeToSplit] = default;
                keyNewValue = newNode.Keys[0];
                BTreeNode<T> newParent = new BTreeNode<T>(order);
                newParent.Children[0] = parentNode;
                DivideChild(ref newParent, 0, keyNewValue);
                newParent.Children[1] = newNode;
                node = newParent;
            }
            else
            {
                for (int i = 0; i < nodeToSplit.Keys.Length; i++)
                {
                    if (key.CompareTo(nodeToSplit.Keys[i]) < 0 && i == 0)
                    {
                        keyNewValue = nodeToSplit.Keys[i];
                        nodeToSplit.Keys[i] = key;
                    }
                    else if (key.CompareTo(nodeToSplit.Keys[i]) > 0 && i > 0)
                    {
                        keyNewValue = nodeToSplit.Keys[i];
                        nodeToSplit.Keys[i] = key;
                    }
                }

                int rightNode = indexOfNodeToSplit + 1;
                parentNode.Keys[parentNode.KeyNumber] = keyNewValue;
                parentNode.Children[indexOfNodeToSplit].KeyNumber--;
                parentNode.Children[rightNode] = new BTreeNode<T>(order);
                parentNode.Children[rightNode].Keys[0] = nodeToSplit.Keys[1];
                parentNode.Children[rightNode].KeyNumber++;
                parentNode.Children[indexOfNodeToSplit].Keys[parentNode.Children[indexOfNodeToSplit].KeyNumber] = default;
                parentNode.KeyNumber++;
                node = parentNode;
            }
        }
    }
}
