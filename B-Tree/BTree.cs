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
                    DivideChild(newNode, ref key);
                    NodeWithFreeSpaces(newNode, key);
                    root = newNode;
                }
            }
        }

        private void NodeWithFreeSpaces(BTreeNode<T> node, T key)
        {
            int indexKeyInNod = 0;
            T temp;
            bool increasingli = false;

            if (node.IsLeaf)
            {
                while (!increasingli && key.CompareTo(node.Keys[indexKeyInNod]) < 0)
                {
                    temp = node.Keys[indexKeyInNod];
                    node.Keys[indexKeyInNod + 1] = temp;
                    indexKeyInNod--;
                    increasingli = true;
                }

                node.Keys[node.KeyNumber + indexKeyInNod] = key;
                node.KeyNumber++;
            }
            else
            {
                while (node.KeyNumber > indexKeyInNod && key.CompareTo(node.Keys[indexKeyInNod]) > 0)
                {
                    indexKeyInNod++;
                }

                if (node.Children[indexKeyInNod].KeyNumber == order - 1)
                {
                    BTreeNode<T> newnode = new BTreeNode<T>(order);
                    newnode.Children[0] = node.Children[indexKeyInNod];
                    DivideChild(newnode, ref key);
                    node.Keys[node.KeyNumber] = key;
                    node.KeyNumber++;
                    node.Children[indexKeyInNod] = newnode.Children[0];
                    node.Children[indexKeyInNod + 1] = newnode.Children[1];
                }
                else if (node.KeyNumber != 0)
                {
                    NodeWithFreeSpaces(node.Children[indexKeyInNod], key);
                }

                if (node.KeyNumber == 0)
                {
                    node.Keys[node.KeyNumber] = key;
                    node.KeyNumber++;
                }
            }
        }

        private void DivideChild(BTreeNode<T> node, ref T key)
        {
            T keyNewValue = key;
            BTreeNode<T> child = node.Children[0];
            for (int i = 0; i < child.Keys.Length; i++)
            {
                if (key.CompareTo(child.Keys[i]) < 0 && i == 0)
                {
                    T temp = child.Keys[i];
                    child.Keys[i] = key;
                    keyNewValue = temp;
                }
                else if (key.CompareTo(child.Keys[i]) > 0 && i > 0)
                {
                    T temp = child.Keys[i];
                    child.Keys[i] = key;
                    keyNewValue = temp;
                }
            }

            key = keyNewValue;
            for (int j = 0; j < child.Keys.Length; j++)
            {
                node.Children[j] = new BTreeNode<T>(order);
                NodeWithFreeSpaces(node.Children[j], child.Keys[j]);
            }
        }
    }
}