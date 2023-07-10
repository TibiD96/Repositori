namespace BinaryTreeCollection
{
    public class BTreeNode<T>
    {
        public List<T>[] Keys;

        public List<BTreeNode<T>>[] Children;

        public BTreeNode(int order)
        {
            Keys = new List<T>[order - 1];
            Children = new List<BTreeNode<T>>[order];
        }
    }
}