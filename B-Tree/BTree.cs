using System;
using System.Security.Cryptography;
using System.Xml.Linq;

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
                    DivideChild(ref this.Root, 0, oldRoot);
                }
            }
            else
            {
                NodeWithFreeSpaces(ref this.Root, key);
                if (this.Root.KeyNumber == 3)
                {
                    BTreeNode<T> oldNode = this.Root;
                    this.Root = new BTreeNode<T>(order);
                    this.Root.Children[0] = oldNode;
                    DivideChild(ref this.Root, 0, oldNode);
                }
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
                if (newnode.KeyNumber == order - 1)
                {
                    newnode.Keys[newnode.KeyNumber] = key;
                    DivideChild(ref node, indexKeyInNod, newnode);
                }
                else
                {
                    if (indexKeyInNod > 0)
                    {
                        newnode.Keys[newnode.KeyNumber] = key;
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

            if (node.Children[indexKeyInNod].KeyNumber == 3)
            {
                BTreeNode<T> oldNode = node;
                node = new BTreeNode<T>(order);
                node.Children[0] = oldNode;
                DivideChild(ref node, 1, oldNode);
            }
        }

        private void DivideChild(ref BTreeNode<T> parentNode, int indexOfNodeToSplit, BTreeNode<T> nodetoBeSplited)
        {
            BTreeNode<T> newNode = new BTreeNode<T>(order);
            parentNode.Keys[indexOfNodeToSplit] = nodetoBeSplited.Keys[(order - 1) / 2];
            nodetoBeSplited.Keys[(order - 1) / 2] = default(T);
            parentNode.Children[parentNode.KeyNumber + 1] = newNode;
            parentNode.KeyNumber++;

            newNode.Keys[0] = nodetoBeSplited.Keys[order - 1];
            newNode.KeyNumber++;
            nodetoBeSplited.Keys[order - 1] = default(T);

            if (!nodetoBeSplited.IsLeaf)
            {
                for (int i = order - 1; i <= order; i++)
                {
                    newNode.Children[i - 2] = nodetoBeSplited.Children[i];
                    nodetoBeSplited.Children[i] = null;
                }
            }

            nodetoBeSplited.KeyNumber--;
        }
    }
}
