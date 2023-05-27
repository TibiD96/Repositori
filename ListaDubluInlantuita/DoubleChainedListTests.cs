using System.Collections.Generic;
using Xunit;

namespace ChainedList
{
    public class DoubleChainedListTests
    {
        [Fact]

        public void CheckIfConstructorWorksDefault()
        {
            var linkedList = new DoubleChainedList<int>();
            Assert.Empty(linkedList);
        }

        [Fact]

        public void CheckIfConstructorWorksWithInputArray()
        {
            int[] input = new[] {1, 2, 3};
            var linkedList = new DoubleChainedList<int>(input);
            Assert.Equal(1, linkedList.GetFirst.Value);
            Assert.Equal(3, linkedList.GetLast.Value);
            Assert.Equal(3, linkedList.Count);
        }

        [Fact]

        public void CheckIfAddAndAddLastMethodesWork()
        {
            int[] input = new[] { 1, 2, 3 };
            var linkedList = new DoubleChainedList<int>(input);
            int numberToAdd = 10;
            linkedList.Add(numberToAdd);
            Assert.Equal(10, linkedList.GetLast.Value);
            Assert.Equal(4, linkedList.Count);
        }

        [Fact]

        public void CheckIfAddLasMethodesWorkWithNode()
        {
            int[] input = new[] { 1, 2, 3 };
            var linkedList = new DoubleChainedList<int>(input);
            int secondInput = 4;
            var secondLinkedList = new LinkedListNode<int>(secondInput);
            linkedList.AddLast(secondLinkedList);
            Assert.Equal(4, linkedList.GetLast.Value);
            Assert.Equal(4, linkedList.Count);
        }

        [Fact]

        public void CheckIfAddBeforeWithLinkedListNodeMethodesWork()
        {
            var linkedList = new DoubleChainedList<int>();
            var add = new LinkedListNode<int>(5);
            var first = new LinkedListNode<int>(1);
            var second = new LinkedListNode<int>(2);
            var third = new LinkedListNode<int>(3);
            var fourth = new LinkedListNode<int>(4);
            linkedList.AddLast(first);
            linkedList.AddLast(second);
            linkedList.AddLast(third);
            linkedList.AddLast(fourth);
            linkedList.AddBefore(first, add);
            Assert.Equal(5, linkedList.GetFirst.Value);
            Assert.Equal(5, linkedList.Count);
        }

        [Fact]

        public void CheckIfAddBeforeWithLinkedListNodeAndElementMethodesWork()
        {
            var linkedList = new DoubleChainedList<int>();
            int add = 5;
            var first = new LinkedListNode<int>(1);
            var second = new LinkedListNode<int>(2);
            var third = new LinkedListNode<int>(3);
            var fourth = new LinkedListNode<int>(4);
            linkedList.AddLast(first);
            linkedList.AddLast(second);
            linkedList.AddLast(third);
            linkedList.AddLast(fourth);
            linkedList.AddBefore(first, add);
            Assert.Equal(5, linkedList.GetFirst.Value);
            Assert.Equal(5, linkedList.Count);
        }

        [Fact]
        public void CheckIfClearMethodesWork()
        {
            int[] input = new[] { 1, 2, 3 };
            var linkedList = new DoubleChainedList<int>(input);
            linkedList.Clear();
            Assert.Empty(linkedList);
        }

        [Fact]
        public void CheckIfFindMethodesWork()
        {
            int[] input = new[] { 1, 2, 3 };
            var linkedList = new DoubleChainedList<int>(input);
            Assert.True(linkedList.Find(2));
            Assert.False(linkedList.Find(5));
        }

        [Fact]
        public void CheckIfFindLastMethodesWork()
        {
            int[] input = new[] { 1, 2, 3 };
            var linkedList = new DoubleChainedList<int>(input);
            Assert.True(linkedList.FindLast(3));
            Assert.False(linkedList.FindLast(5));
        }

        [Fact]
        public void CheckIfContainMethodesWork()
        {
            int[] input = new[] { 1, 2, 3 };
            var linkedList = new DoubleChainedList<int>(input);
            Assert.Contains(3, linkedList);
            Assert.DoesNotContain(5, linkedList);
        }

        [Fact]

        public void CheckIfAddAfterWithLinkedListNodeMethodesWork()
        {
            var linkedList = new DoubleChainedList<int>();
            var add = new LinkedListNode<int>(5);
            var first = new LinkedListNode<int>(1);
            var second = new LinkedListNode<int>(2);
            var third = new LinkedListNode<int>(3);
            var fourth = new LinkedListNode<int>(4);
            linkedList.AddLast(first);
            linkedList.AddLast(second);
            linkedList.AddLast(third);
            linkedList.AddLast(fourth);
            linkedList.AddAfter(first, add);
            Assert.Equal(5, linkedList.GetFirst.Right.Value);
            Assert.Equal(2, linkedList.GetFirst.Right.Right.Value);
            Assert.Equal(5, linkedList.Count);
        }

        [Fact]

        public void CheckIfAddAfterWithLinkedListNodeAndElementMethodesWork()
        {
            var linkedList = new DoubleChainedList<int>();
            int add = 5;
            var first = new LinkedListNode<int>(1);
            var second = new LinkedListNode<int>(2);
            var third = new LinkedListNode<int>(3);
            var fourth = new LinkedListNode<int>(4);
            linkedList.AddLast(first);
            linkedList.AddLast(second);
            linkedList.AddLast(third);
            linkedList.AddLast(fourth);
            linkedList.AddAfter(first, add);
            Assert.Equal(5, linkedList.GetFirst.Right.Value);
            Assert.Equal(5, linkedList.Count);
        }

        [Fact]
        public void CheckIfAddFirstWithLinkedListNodeMethodesWork()
        {
            int[] input = new[] { 1, 2, 3 };
            var linkedList = new DoubleChainedList<int>(input);
            var add = new LinkedListNode<int>(5);
            linkedList.AddFirst(add);
            Assert.Equal(5, linkedList.GetFirst.Value);
        }

        [Fact]
        public void CheckIfAddFirstWithElementMethodesWork()
        {
            int[] input = new[] { 1, 2, 3 };
            var linkedList = new DoubleChainedList<int>(input);;
            linkedList.AddFirst(5);
            Assert.Equal(5, linkedList.GetFirst.Value);
        }

        [Fact]
        public void CheckIfRemoveMethodesWork()
        {
            int[] input = new[] { 1, 2, 3 };
            var linkedList = new DoubleChainedList<int>(input);
            var remove = new LinkedListNode<int>(2);
            linkedList.Remove(remove);
            Assert.Equal(2, linkedList.Count);
        }


    }
}
