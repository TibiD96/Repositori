using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Linq
{
    public class FilteringFunctiosTests
    {
        [Fact]

        public void AtLeastOneFeature()
        {
            var inputListProd = new List<ProductWithFeature>()
            {
                new ProductWithFeature("RcCar", 1, 2, 3, 4, 5),
                new ProductWithFeature("Phone", 3, 5),
                new ProductWithFeature("Tv", 10, 25),
                new ProductWithFeature("Ball", 2)
            };

            var inputFeatures = new List<Feature>()
            {
                new Feature(1),
                new Feature(5),
                new Feature(2)
            };

            var expected = new List<ProductWithFeature>()
            {
                new ProductWithFeature("RcCar", 1, 2, 3, 4, 5),
                new ProductWithFeature("Phone", 3, 5),
                new ProductWithFeature("Ball", 2)
            };

            var final = FilteringFunctions.AtLeastOneFeature(inputListProd, inputFeatures);

            Assert.Equal(expected, final);
        }

        [Fact]

        public void ALLFeature()
        {
            var inputListProd = new List<ProductWithFeature>()
            {
                new ProductWithFeature("RcCar", 1, 2, 3, 4, 5),
                new ProductWithFeature("Phone", 3, 5),
                new ProductWithFeature("Tv", 10, 25),
                new ProductWithFeature("Ball", 2)
            };

            var inputFeatures = new List<Feature>()
            {
                new Feature(1),
                new Feature(5),
                new Feature(2)
            };

            var expected = new List<ProductWithFeature>()
            {
                new ProductWithFeature("RcCar", 1, 2, 3, 4, 5)
            };

            var final = FilteringFunctions.AllFeature(inputListProd, inputFeatures);

            Assert.Equal(expected, final);
        }

        [Fact]

        public void NoFeature()
        {
            var inputListProd = new List<ProductWithFeature>()
            {
                new ProductWithFeature("RcCar", 1, 2, 3, 4, 5),
                new ProductWithFeature("Phone", 3, 5),
                new ProductWithFeature("Tv", 10, 25),
                new ProductWithFeature("Ball", 2)
            };

            var inputFeatures = new List<Feature>()
            {
                new Feature(1),
                new Feature(5),
                new Feature(2)
            };

            var expected = new List<ProductWithFeature>()
            {
                new ProductWithFeature("Tv", 10, 25)
            };

            var final = FilteringFunctions.NoFeature(inputListProd, inputFeatures);

            Assert.Equal(expected, final);
        }

        [Fact]

        public void ProdQuantity()
        {
            var firstInputListProd = new List<Product>()
            {
                new Product("RcCar", 50),
                new Product("Phone", 4),
                new Product("TV", 20),
                new Product("Table", 6),
                new Product("Ball", 2)
            };

            var secondInputListProd = new List<Product>()
            {
                new Product("Phone", 6),
                new Product("PS5", 10),
                new Product("PC", 6),
                new Product("RcCar", 5)
            };

            var expected = new List<Product>()
            {
                new Product("RcCar", 55),
                new Product("Phone", 10),
                new Product("TV", 20),
                new Product("Table", 6),
                new Product("Ball", 2),
                new Product("PS5", 10),
                new Product("PC", 6)
            };

            var final = FilteringFunctions.ProductQuantity(firstInputListProd, secondInputListProd);

            Assert.Equal(expected, final);
        }

        [Fact]

        public void ProdQuantitySecondTest()
        {
            var firstInputListProd = new List<Product>()
            {
                new Product("RcCar", 50),
                new Product("Phone", 4),
                new Product("TV", 20),
                new Product("Table", 6),
                new Product("Ball", 2)
            };

            var secondInputListProd = new List<Product>()
            {
                new Product("Phone", 4),
                new Product("PS5", 10),
                new Product("PC", 6),
                new Product("RcCar", 50)
            };

            var expected = new List<Product>()
            {
                new Product("RcCar", 100),
                new Product("Phone", 8),
                new Product("TV", 20),
                new Product("Table", 6),
                new Product("Ball", 2),
                new Product("PS5", 10),
                new Product("PC", 6)
            };

            var final = FilteringFunctions.ProductQuantity(firstInputListProd, secondInputListProd);

            Assert.Equal(expected, final);
        }

        [Fact]

        public void RankingResultsTest()
        {
            var inputRanking = new List<TestResults>()
            {
                new TestResults("1", "Ana", 5),
                new TestResults("2", "Mihai", 4),
                new TestResults("3", "Ilie", 20),
                new TestResults("4", "Ana", 10),
                new TestResults("5", "Mihai", 1),
                new TestResults("6", "George", 1),
                new TestResults("7", "Ana", 5),
                new TestResults("8", "Ilie", 20)
            };

            var expected = new List<TestResults>()
            {
                new TestResults("4", "Ana", 10),
                new TestResults("2", "Mihai", 4),
                new TestResults("3", "Ilie", 20),
                new TestResults("6", "George", 1)
            };

            var final = FilteringFunctions.RankingResult(inputRanking);

            Assert.Equal(expected, final);
        }

        [Fact]

        public void WordRankingTest()
        {
            const string input = "After a long long and hard day, Alex is ready for a long and intens gaming with Andrei and Alex.";

            List<(int, string)> expected = new ()
            {
                (3, "long"),
                (3, "and"),
                (2, "a"),
                (2, "alex"),
                (1, "after"),
                (1, "hard"),
                (1, "day"),
                (1, "is"),
                (1, "ready"),
                (1, "for"),
                (1, "intens"),
                (1, "gaming"),
                (1, "with"),
                (1, "andrei")
            };

            var final = FilteringFunctions.WordRanking(input);

            Assert.Equal(expected, final);
        }

        [Fact]

        public void CheckSudokuToBeValid()
        {
            int[,] inputSudokuTable = new int[,]
            {
                { 5, 3, 4, 6, 7, 8, 9, 1, 2 },
                { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
                { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
                { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
                { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
                { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
                { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
                { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
                { 3, 4, 5, 2, 8, 6, 1, 7, 9 }
            };

            Assert.True(FilteringFunctions.SudokuValidator(inputSudokuTable));
        }

        [Fact]

        public void CheckSudokuToBeInvalid()
        {
            int[,] inputSudokuTable = new int[,]
            {
                { 5, 3, 4, 1, 7, 8, 9, 1, 2 },
                { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
                { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
                { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
                { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
                { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
                { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
                { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
                { 3, 4, 5, 2, 8, 6, 1, 7, 9 }
            };

            Assert.False(FilteringFunctions.SudokuValidator(inputSudokuTable));
        }

        [Fact]

        public void CheckPostfixEquation()
        {
            const string input = "3 4 * 2 5 * +";

            const double expected = 22;

            var final = FilteringFunctions.PostfixEquation(input);

            Assert.Equal(expected, final);
        }
    }
}
