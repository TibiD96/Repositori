using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class IntArrayFunctions
    {
       public static List<List<int>> SumLowerEqualThenK(int[] intArray, int k)
       {
            return Enumerable.Range(0, intArray.Length).SelectMany(startIndex => Enumerable.Range(1, intArray.Length - startIndex)
                                                       .Select(length => intArray.Skip(startIndex).Take(length).ToList())
                                                       .TakeWhile(subList => subList.Sum() <= k)).ToList();
       }

       public static List<List<int>> ValidCombination(int lastNumber, int result)
       {
            List<List<int>> listValidComb = new List<List<int>> { new List<int>() };

            return Enumerable.Range(1, lastNumber).Aggregate(listValidComb, (element, i) => element
                             .SelectMany(comb => new[] { new List<int>(comb) { i }, new List<int>(comb) { -i } }).ToList())
                             .Where(comb => comb.Sum() == result).ToList();
       }

       public static List<List<int>> ValidTriplets(int[] inputArray)
       {
            const int constantLength = 3;
            return Enumerable.Range(0, inputArray.Length - constantLength + 1).Select(startingIndex => inputArray.Skip(startingIndex).Take(constantLength).ToList())
                                                                              .Where(subList => Math.Pow(subList[0], 2) + Math.Pow(subList[1], 2) == Math.Pow(subList[2], 2)).ToList();
       }
    }
}