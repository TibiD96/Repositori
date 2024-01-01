using System;
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
            for (int i = 1; i <= lastNumber; i++)
            {
                listValidComb = listValidComb.SelectMany(comb => new[] { new List<int>(comb) { i }, new List<int>(comb) { -i } }).ToList();
            }

            return listValidComb.Where(comb => comb.Sum() == result).ToList();
       }
    }
}