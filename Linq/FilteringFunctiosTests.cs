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
    }
}
