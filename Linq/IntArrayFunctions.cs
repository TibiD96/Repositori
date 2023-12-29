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

       public static List<List<int>> ValidCombination(int numberOfElements, int result)
       {

       }
    }
}
