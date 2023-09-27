using System;

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
                if (this.Root.KeyNumber < order)
                {
                    this.Root.Keys[this.Root.KeyNumber] = key;
                    this.Root.KeyNumber++;
                }

                if (this.Root.KeyNumber == order)
                {
                    OrderKeys(ref this.Root);
                }
            }
            else
            {
                NodeWithFreeSpaces(ref this.Root, key);
            }

            if (this.Root.KeyNumber != 3)
            {
                return;
            }

            BTreeNode<T> oldNode = this.Root;
            this.Root = new BTreeNode<T>(order);
            this.Root.Children[0] = oldNode;
            DivideChild(ref this.Root, 0, oldNode);
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
                    newnode.KeyNumber++;
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

            if (node.Children[indexKeyInNod].KeyNumber != 3)
            {
                return;
            }

            BTreeNode<T> oldNode = node.Children[indexKeyInNod];
            DivideChild(ref node, indexKeyInNod, oldNode);
        }

        private void DivideChild(ref BTreeNode<T> parentNode, int indexOfNodeToSplit, BTreeNode<T> nodetoBeSplited)
        {
            BTreeNode<T> newNode = new BTreeNode<T>(order);
            parentNode.Keys[indexOfNodeToSplit] = nodetoBeSplited.Keys[(order - 1) / 2];
            nodetoBeSplited.Keys[(order - 1) / 2] = default(T);
            nodetoBeSplited.KeyNumber--;
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

        private void OrderKeys(ref BTreeNode<T> node)
        {
            while (node.Keys[0].CompareTo(node.Keys[1]) > 0 || node.Keys[1].CompareTo(node.Keys[order - 1]) > 0)
            {
                if (node.Keys[0].CompareTo(node.Keys[1]) > 0)
                {
                    T temp = node.Keys[1];
                    node.Keys[1] = node.Keys[0];
                    node.Keys[0] = temp;
                }

                if (node.Keys[1].CompareTo(node.Keys[2]) > 0)
                {
                    T temp = node.Keys[2];
                    node.Keys[2] = node.Keys[1];
                    node.Keys[1] = temp;
                }
            }
        }
    }
}
