using System;
using System.Reflection;

namespace BinaryTreeCollection
{
    public class BTreeGraph<T> where T : IComparable<T>
    {
        private BTreeNode<T> Root;

        private readonly int order;

        public BTreeGraph(int order)
        {
            this.Root = new BTreeNode<T>(order);
            this.order = order;
        }

        public int Count { get; set; }

        public void Add(T key)
        {
            if (this.Root.IsLeaf)
            {
                if (this.Root.KeyNumber < order - 1)
                {
                    this.Root.Keys[this.Root.KeyNumber] = key;
                    this.Root.KeyNumber++;
                }
                else
                {
                    this.Root.Keys[this.Root.KeyNumber] = key;
                    BTreeNode<T> oldRoot = this.Root;
                    this.Root = new BTreeNode<T>(order);
                    this.Root.Children[0] = oldRoot;
                    DivideChild(this.Root, 0, oldRoot);
                }
            }
            else
            {
                NodeWithFreeSpaces(this.Root, key);
            }
        }

        private void NodeWithFreeSpaces(BTreeNode<T> node, T key)
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
                     DivideChild(node, indexKeyInNod, newnode);
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
                NodeWithFreeSpaces(node.Children[indexKeyInNod], key);
            }
        }

        private void DivideChild(BTreeNode<T> parentNode, int indexOfNodeToSplit, BTreeNode<T> nodetoBeSplited)
        {
            BTreeNode<T> newNode = new BTreeNode<T>(order);
            parentNode.Keys[indexOfNodeToSplit] = nodetoBeSplited.Keys[order - 1];
            parentNode.Children[parentNode.KeyNumber + 1] = newNode;
            parentNode.KeyNumber++;

            for (int j = 0; j < order - 1; j++)
            {
                newNode.Keys[j] = nodetoBeSplited.Keys[j + order];
                nodetoBeSplited.Keys[j + order] = default(T);
            }

            if (!nodetoBeSplited.IsLeaf)
            {
                for (int j = 0; j < order; j++)
                {
                    newNode.Children[j] = nodetoBeSplited.Children[j + order];
                    nodetoBeSplited.Children[j + order] = null;
                }
            }

            nodetoBeSplited.KeyNumber = order - 1;
        }
    }
}
