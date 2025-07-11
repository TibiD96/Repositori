﻿namespace BinaryTreeCollection
{
    public class BTreeNode<T>
    {
        public T[] Keys;

        public BTreeNode<T>[] Children;

        public int KeyNumber;

        public BTreeNode(int order)
        {
            KeyNumber = 0;
            Keys = new T[order];
            Children = new BTreeNode<T>[order + 1];
        }

        public bool IsLeaf
        {
            get { return Children[0] == null; }
        }
    }
}