using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class IntArrayFunctions
    {
       public static IEnumerable<IEnumerable<int>> SumLowerEqualThenK(int[] intArray, int k)
       {
            return Enumerable.Range(0, intArray.Length).SelectMany(startIndex => Enumerable.Range(1, intArray.Length - startIndex)
                                                       .Select(length => intArray.Skip(startIndex).Take(length))
                                                       .TakeWhile(subList => subList.Sum() <= k));
       }

       public static IEnumerable<IEnumerable<int>> ValidCombination(int lastNumber, int result)
       {
            IEnumerable<IEnumerable<int>> listValidComb = new[] { new int[] { } };

            return Enumerable.Range(1, lastNumber).Aggregate(listValidComb, (element, i) => element
                             .SelectMany(comb => new[] { new List<int>(comb) { i }, new List<int>(comb) { -i } }))
                             .Where(comb => comb.Sum() == result);
       }

       public static IEnumerable<IEnumerable<int>> ValidTriplets(int[] inputArray)
       {
            return inputArray.SelectMany((firstNumber, firstIndex) => inputArray.Skip(firstIndex + 1)
                             .SelectMany((secondNumber, secondIndex) => inputArray.Skip(firstIndex + secondIndex + 1)
                             .Select(thirdNumber => new List<int> { firstNumber, secondNumber, thirdNumber })))
                             .Where(subList => Math.Pow(subList[0], 2) + Math.Pow(subList[1], 2) == Math.Pow(subList[2], 2) ||
                                               Math.Pow(subList[2], 2) + Math.Pow(subList[0], 2) == Math.Pow(subList[1], 2) ||
                                               Math.Pow(subList[1], 2) + Math.Pow(subList[2], 2) == Math.Pow(subList[0], 2))
                              .Select(triplet => triplet.OrderBy(x => x));
       }
    }
}