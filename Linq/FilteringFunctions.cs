using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class FilteringFunctions
    {
        public static List<ProductWithFeature> AtLeastOneFeature(List<ProductWithFeature> inputListProd, List<Feature> inputListFeature)
        {
            return inputListProd.Where(prod => prod.Features.Intersect(inputListFeature).Any()).ToList();
        }

        public static List<ProductWithFeature> AllFeature(List<ProductWithFeature> inputListProd, List<Feature> inputListFeature)
        {
            return inputListProd.Where(prod => inputListFeature.All(feature => prod.Features.Contains(feature))).ToList();
        }

        public static List<ProductWithFeature> NoFeature(List<ProductWithFeature> inputListProd, List<Feature> inputListFeature)
        {
            return inputListProd.Where(prod => inputListFeature.All(feature => !prod.Features.Contains(feature))).ToList();
        }

        public static List<Product> ProductQuantity(List<Product> firstInputListProd, List<Product> secondInputListProd)
        {
            return firstInputListProd.Union(secondInputListProd).GroupBy(product => product.Name)
                                     .Select(prod =>
                                     {
                                         int sumQuant = prod.Aggregate(0, (result, product) => result + product.Quantity);
                                         return new Product(prod.Key, sumQuant);
                                     }).ToList();
        }
    }
}
