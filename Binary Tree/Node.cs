namespace BinaryTreeCollection
{
    public class Node
    {
        public int Value;
        public Node Left;
        public Node Right;

        public Node(int data)
        {
            this.Value = data;
            this.Left = null;
            this.Right = null;
        }
    }
}