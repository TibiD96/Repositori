using System;
using System.Security.Cryptography;

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
                    int indexKeyInNod = 0;

                    bool ordered = false;
                    while (!ordered)
                    {
                        if (key.CompareTo(root.Keys[indexKeyInNod]) < 0)
                        {
                            T temp = root.Keys[indexKeyInNod];
                            root.Keys[indexKeyInNod] = key;
                            root.Keys[root.KeyNumber] = temp;
                            root.KeyNumber++;
                            ordered = true;
                        }

                        indexKeyInNod++;
                    }
                }
            }
        }
    }
}
