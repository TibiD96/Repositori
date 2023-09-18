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
                    newnode.Keys[indexOfNewKey] = key;
                    DivideChild(ref node, indexKeyInNod, newnode);
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
                    newNode.Children[0] = nodetoBeSplited.Children[i];
                    nodetoBeSplited.Children[i] = null;
                }
            }

            nodetoBeSplited.KeyNumber--;

            if (parentNode.KeyNumber != order)
            {
                return;
            }

            BTreeNode<T> oldParent = parentNode;
            parentNode = new BTreeNode<T>(order);
            parentNode.Keys[0] = oldParent.Keys[(order - 1) / 2];
            parentNode.KeyNumber++;
            BTreeNode<T> leftHalf = new BTreeNode<T>(order);
            leftHalf.Keys[0] = oldParent.Keys[(order / 2) - 1];
            leftHalf.KeyNumber++;
            leftHalf.Children[0] = oldParent.Children[0];
            leftHalf.Children[1] = oldParent.Children[1];
            BTreeNode<T> rightHalf = new BTreeNode<T>(order);
            rightHalf.Keys[0] = oldParent.Keys[order - 1];
            rightHalf.KeyNumber++;
            rightHalf.Children[0] = oldParent.Children[order - 1];
            rightHalf.Children[1] = oldParent.Children[order];
            parentNode.Children[0] = leftHalf;
            parentNode.Children[1] = rightHalf;
        }
    }
}
