﻿using Xunit;

namespace CollectionData
{
    public class IntArrayTests
    {
        [Fact]

        public void AddElemetsInArrayPositioningItOnTheEnd()
        {
            var input = new IntArray();
            input.Add(5);
            Assert.Equal(1, input.Count);
            Assert.Equal(5, input[input.Count - 1]);
        }

        [Fact]

        public void ChangeValueOfTheElementFromGivenIndex()
        {
            var input = new IntArray();
            int correctValueAfterChanging = 1000;
            input[0] = 1000;
            Assert.Equal(input[0], correctValueAfterChanging);
        }

        [Fact]

        public void ReturnTrueIfElementIsInArray()
        {
            var input = new IntArray();
            int numbertoCheck = 1000;
            input.Add(5);
            input.Add(10);
            input.Add(1000);
            input.Add(25);
            Assert.True(input.Contains(numbertoCheck));
        }

        [Fact]

        public void ReturnFalseIfElementIsNotInArray()
        {
            var input = new IntArray();
            int numbertoCheck = 1000;
            input.Add(5);
            input.Add(10);
            input.Add(30);
            input.Add(25);
            Assert.False(input.Contains(numbertoCheck));
        }

        [Fact]

        public void ReturnTheIndexOfGivenNumberElseReturnMinusOne()
        {
            var input = new IntArray();
            input.Add(5);
            input.Add(10);
            input.Add(40);
            input.Add(25);
            Assert.Equal(2, input.IndexOf(40));
            Assert.Equal(-1, input.IndexOf(100));
        }

        [Fact]

        public void InsertTheNumberToTheSpecifiedPosition()
        {
            var input = new IntArray();
            input.Add(2);
            input.Add(1);
            input.Add(2);
            input.Add(3);
            input.Insert(2, 3);
            Assert.Equal(3, input[2]);
            Assert.Equal(2, input[3]);
        }

        [Fact]

        public void DeleteAllElementsFfromArray()
        {
            var input = new IntArray();
            input.Add(0);
            input.Add(1);
            input.Add(2);
            input.Add(3);
            input.Clear();
            Assert.Equal(0, input.Count);
        }

        [Fact]

        public void DeleteTheFirstAppearenceOfTheGivenElement()
        {
            var input = new IntArray();
            int givenElelement = 3;
            input.Add(2);
            input.Add(1);
            input.Add(3);
            input.Add(9);
            input.Add(3);
            input.Add(5);
            input.Remove(givenElelement);
            Assert.Equal(5, input.Count);
            Assert.Equal(9, input[2]);
        }

        [Fact]

        public void DeleteTheElementFromTheGivenIndex()
        {
            var input = new IntArray();
            int givenIndex = 3;
            input.Add(7);
            input.Add(1);
            input.Add(4);
            input.Add(9);
            input.Add(3);
            input.RemoveAt(givenIndex);
            Assert.Equal(4, input.Count);
            Assert.Equal(3, input[3]);

        }
    }
}