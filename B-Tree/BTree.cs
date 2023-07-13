using System;
using System.Security.Cryptography;
using System.Xml.Linq;

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
                    DivideChild(newNode, key);
                }
            }
        }

        private void NodeWithFreeSpaces(BTreeNode<T> node, T key)
        {
            int indexKeyInNod = 0;

            bool ordered = false;
            while (!ordered)
            {
                if (key.CompareTo(node.Keys[indexKeyInNod]) < 0)
                {
                    T temp = node.Keys[indexKeyInNod];
                    root.Keys[indexKeyInNod] = key;
                    root.Keys[node.KeyNumber] = temp;
                    root.KeyNumber++;
                    ordered = true;
                }
                else if (indexKeyInNod == node.KeyNumber)
                {
                    node.Keys[node.KeyNumber] = key;
                    node.KeyNumber++;
                    ordered = true;
                }

                indexKeyInNod++;
            }
        }

        private void DivideChild(BTreeNode<T> node, T key)
        {
            for (int i = 0; i < node.Children.Length; i++)
            {
                //if (key.CompareTo(node.Children[i]))
            }
        }
    }
}
