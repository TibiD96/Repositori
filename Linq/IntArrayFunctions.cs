using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class IntArrayFunctions
    {
       public static List<List<int>> SumLowerEqualThenK(int[] intArray, int k)
        {
            List<List<int>> list = new List<List<int>>();
            for (int i = 0; i < intArray.Length; i++)
            {
                List<int> subList = new List<int>();
                int currentSum = 0;

                for (int j = i + 1; j <= intArray.Length; j++)
                {
                    foreach (int c in intArray[i..j])
                    {
                        currentSum += c;
                    }

                    if (currentSum <= k)
                    {
                        subList.AddRange(intArray[i..j]);
                        list.Add(subList);
                        currentSum = 0;
                    }

                    subList = new List<int>();
                }
            }

            return list;
        }
    }
}
