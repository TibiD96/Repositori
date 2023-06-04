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
            Assert.Null(linkedList.First);
        }

        [Fact]

        public void CheckIfConstructorWorksWithInputArray()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            Assert.Equal(1, linkedList.First.Value);
            Assert.Equal(3, linkedList.Last.Value);
            Assert.Equal(3, linkedList.Count);
        }

        [Fact]

        public void CheckIfAddAndAddLastMethodesWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            int numberToAdd = 10;
            linkedList.Add(numberToAdd);
            Assert.Equal(10, linkedList.Last.Value);
            Assert.Equal(4, linkedList.Count);
        }

        [Fact]

        public void CheckIfAddLasMethodesWorkWithNode()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            int secondInput = 4;
            var secondLinkedList = new LinkedListNode<int>(secondInput);
            linkedList.AddLast(secondLinkedList);
            Assert.Equal(4, linkedList.Last.Value);
            Assert.Equal(4, linkedList.Count);
        }

        [Fact]

        public void CheckIfAddBeforeWithLinkedListNodeMethodesWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3, 4};
            var add = new LinkedListNode<int>(5);
            linkedList.AddBefore(linkedList.First.Right, add);
            Assert.Equal(5, linkedList.First.Right.Value);
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
            Assert.Equal(5, linkedList.First.Value);
            Assert.Equal(5, linkedList.Count);
        }

        [Fact]
        public void CheckIfClearMethodesWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            linkedList.Clear();
            Assert.Empty(linkedList);
        }

        [Fact]
        public void CheckIfFindMethodesWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            Assert.Equal(linkedList.First.Right, linkedList.Find(2));
            Assert.Null(linkedList.Find(5));
        }

        [Fact]
        public void CheckIfFindLastMethodesWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            Assert.Equal(linkedList.Last, linkedList.FindLast(3));
            Assert.Null(linkedList.FindLast(5));
        }

        [Fact]
        public void CheckIfContainMethodesWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
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
            Assert.Equal(5, linkedList.First.Right.Value);
            Assert.Equal(2, linkedList.First.Right.Right.Value);
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
            Assert.Equal(5, linkedList.First.Right.Value);
            Assert.Equal(5, linkedList.Count);
        }

        [Fact]
        public void CheckIfAddFirstWithLinkedListNodeMethodesWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            var add = new LinkedListNode<int>(5);
            linkedList.AddFirst(add);
            Assert.Equal(5, linkedList.First.Value);
        }

        [Fact]
        public void CheckIfAddFirstWithElementMethodesWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            linkedList.AddFirst(5);
            Assert.Equal(5, linkedList.First.Value);
        }

        [Fact]
        public void CheckIfRemoveMethodesWorkForListWithElements()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            int remove = 2;
            linkedList.Remove(remove);
            Assert.Equal(2, linkedList.Count);
            Assert.Equal(3, linkedList.First.Right.Value);
        }

        [Fact]
        public void CheckIfRemoveMethodesWorkForEmptyList()
        {
            var linkedList = new DoubleChainedList<int> {};
            int remove = 2;
            linkedList.Remove(remove);
            Assert.False(linkedList.Remove(remove));
        }

        [Fact]
        public void CheckIfRemoveFirstMethodesWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            linkedList.RemoveFirst();
            Assert.Equal(2, linkedList.Count);
            Assert.Equal(2, linkedList.First.Value);
        }

        [Fact]
        public void CheckIfRemoveFirstMethodesWorkForEmptyList()
        {
            var linkedList = new DoubleChainedList<int> {};
            Assert.Equal(0, linkedList.Count);
            Assert.False(linkedList.RemoveFirst());
        }

        [Fact]
        public void CheckIfRemoveLastMethodesWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3, 5 };
            linkedList.RemoveLast();
            Assert.Equal(3, linkedList.Count);
            Assert.Equal(3, linkedList.Last.Value);
        }

        [Fact]
        public void CheckIfRemoveLastMethodesWorkWithEmptyList()
        {
            var linkedList = new DoubleChainedList<int> {};
            Assert.Equal(0, linkedList.Count);
            Assert.False(linkedList.RemoveLast());
        }

        [Fact]
        public void CheckIfArgumentNullExceptionWorks()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            LinkedListNode<int> nullNode = null;
            Assert.Throws<ArgumentNullException>(() => linkedList.AddFirst(nullNode));
            Assert.Throws<ArgumentNullException>(() => linkedList.AddLast(nullNode));
            Assert.Throws<ArgumentNullException>(() => linkedList.AddAfter(linkedList.First, nullNode));
            Assert.Throws<ArgumentNullException>(() => linkedList.AddBefore(linkedList.Last, nullNode));
            Assert.Throws<ArgumentNullException>(() => linkedList.Remove(nullNode));
        }

        [Fact]
        public void CheckIfArgumentNullExceptionWorksForItem()
        {
            var linkedList = new DoubleChainedList<int?> { 1, 2, 3 };
            int? nullItem = null;
            Assert.Throws<ArgumentNullException>(() => linkedList.AddFirst(nullItem));
            Assert.Throws<ArgumentNullException>(() => linkedList.AddLast(nullItem));
            Assert.Throws<ArgumentNullException>(() => linkedList.AddAfter(linkedList.First, nullItem));
            Assert.Throws<ArgumentNullException>(() => linkedList.AddBefore(linkedList.Last, nullItem));
        }

        [Fact]
        public void CheckIfInvalidOperationExceptionWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            var item = new LinkedListNode<int>(5);
            Assert.Throws<InvalidOperationException>(() => linkedList.AddAfter(item, 8));
            Assert.Throws<InvalidOperationException>(() => linkedList.AddBefore(item, 8));
            Assert.Throws<InvalidOperationException>(() => linkedList.Remove(item));
        }

        [Fact]
        public void CheckICopyTWork()
        {
            var linkedList = new DoubleChainedList<int> { 1, 2, 3 };
            var secondLinkedList = new int[3];
            linkedList.CopyTo(secondLinkedList, 0);
            Assert.Equal(linkedList.First.Value, secondLinkedList[0]);
            Assert.Equal(linkedList.Last.Value, secondLinkedList[2]);

        }
    }
}
