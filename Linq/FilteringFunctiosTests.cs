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
                new TestResults("3", "Mihai", 1),
                new TestResults("5", "Ilie", 20)
            };

            var expected = new List<TestResults>()
            {
                new TestResults("1", "Ana", 10),
                new TestResults("2", "Mihai", 4),
                new TestResults("3", "Ilie", 20)
            };

            var final = FilteringFunctions.RankingResult(inputRanking);

            Assert.Equal(expected, final);
        }
    }
}
