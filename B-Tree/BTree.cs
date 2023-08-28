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
            if (root.IsLeaf && root.KeyNumber == order - 1)
            {
                BTreeNode<T> newNode = new BTreeNode<T>(order);
                newNode.Children[0] = root;
                DivideChild(ref newNode, 0, key);
                root = newNode;
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

            if (node.KeyNumber == 0)
            {
                node.Keys[indexKeyInNod] = key;
                node.KeyNumber++;
                return;
            }

            while (key.CompareTo(node.Keys[indexKeyInNod]) >= 0)
            {
                indexKeyInNod++;
                if (indexKeyInNod == node.KeyNumber)
                {
                    break;
                }
            }

            if (node.IsLeaf)
            {
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
                BTreeNode<T> newnode = node.Children[indexKeyInNod];
                if (newnode.KeyNumber == order - 1)
                {
                    DivideChild(ref node, indexKeyInNod, key);
                    return;
                }

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
