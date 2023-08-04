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

            while (key.CompareTo(node.Keys[indexKeyInNod]) >= 0)
            {
                indexKeyInNod++;
            }

            if (node.IsLeaf)
            {
                node.Keys[indexKeyInNod] = key;
                node.KeyNumber++;
            }
            else
            {
                BTreeNode<T> newnode = node.Children[indexKeyInNod];
                newnode.Children[0] = node.Children[indexKeyInNod];
                if (newnode.KeyNumber == order - 1)
                {
                    DivideChild(node, indexKeyInNod, newnode);
                    if (key.CompareTo(node.Keys[indexKeyInNod]) > 0)
                    {
                        indexKeyInNod++;
                    }
                }

                NodeWithFreeSpaces(node.Children[indexKeyInNod], key);
            }
        }

        private void DivideChild(BTreeNode<T> node, int indexKeyInNod, BTreeNode<T> nodenodeToSplit)
        {
            T keyNewValue = key;
            int childNumber = 0;
            BTreeNode<T> parent = node;
            BTreeNode<T> child = node;
            while (!child.IsLeaf)
            {
                if (key.CompareTo(child.Keys[childNumber]) < 0 && childNumber == 0)
                {
                    parent = child;
                    child = child.Children[0];
                    break;
                }

                if (key.CompareTo(child.Keys[childNumber]) > 0 && key.CompareTo(child.Keys[childNumber + 1]) < 0)
                {
                    parent = child;
                    child = child.Children[1];
                    break;
                }

                parent = child;
                child = child.Children[order - 1];

                childNumber++;
            }

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
