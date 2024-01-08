using System;
using System.Collections;
using System.Collections.Generic;

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
            firstInputListProd.AddRange(secondInputListProd);
            return firstInputListProd.GroupBy(product => product.Name)
                                     .Select(prod =>
                                     {
                                         int sumQuant = prod.Aggregate(0, (result, product) => result + product.Quantity);
                                         return new Product(prod.Key, sumQuant);
                                     }).ToList();
        }

        public static List<TestResults> RankingResult(List<TestResults> inputRanking)
        {
            return inputRanking.GroupBy(fam => fam.FamilyId)
                                     .Select(group =>
                                     {
                                         var maxScoreEntry = group.OrderByDescending(score => score.Score).FirstOrDefault();
                                         return new TestResults(maxScoreEntry.Id, group.Key, maxScoreEntry.Score);
                                     }).ToList();
        }

        public static List<(int, string)> WordRanking(string inputString)
        {
            const string delimitations = " ,.!?:\"";
            return inputString.ToLower().Split(delimitations.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                        .GroupBy(words => words)
                                        .Select(group => (Count: group.Count(), Word: group.Key))
                                        .OrderByDescending(pair => pair.Count).ToList();
        }

        public static bool SudokuValidator(int[,] sudokuTable)
        {
            var rows = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9)
                                             .Select(j => sudokuTable[i, j]).ToArray()).ToArray();

            var column = Enumerable.Range(0, 9).Select(j => Enumerable.Range(0, 9)
                                               .Select(i => sudokuTable[i, j]).ToArray()).ToArray();

            var blocks = Enumerable.Range(0, 3).SelectMany(i => Enumerable.Range(0, 3)
                                               .Select(j => Enumerable.Range(0, 3)
                                               .SelectMany(k => Enumerable.Range(0, 3)
                                               .Select(l => sudokuTable[3 * i + k, 3 * j + l])).ToArray())).ToArray();

            return Enumerable.Range(0, 9).All(i => rows[i].GroupBy(num => num).All(group => group.Count() == 1 && group.Key >= 1 && group.Key <= 9)) &&
                   Enumerable.Range(0, 9).All(i => column[i].GroupBy(num => num).All(group => group.Count() == 1 && group.Key >= 1 && group.Key <= 9)) &&
                   Enumerable.Range(0, 9).All(i => blocks[i].GroupBy(num => num).All(group => group.Count() == 1 && group.Key >= 1 && group.Key <= 9));
        }

        public static double PostfixEquation(string inputEquation)
        {
            string[] arrayOfElements = inputEquation.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Stack<double> stack = new Stack<double>();

            return arrayOfElements.Aggregate(stack, (stack, element) =>
            {
                if (double.TryParse(element, out double number))
                {
                    stack.Push(number);
                }
                else
                {
                    double firstNumber = stack.Pop();
                    double secondNumber = stack.Pop();

                    switch (element)
                    {
                        case "+":
                            stack.Push(firstNumber + secondNumber);
                            break;
                        case "-":
                            stack.Push(firstNumber - secondNumber);
                            break;
                        case "*":
                            stack.Push(firstNumber * secondNumber);
                            break;
                        case "/":
                            stack.Push(firstNumber / secondNumber);
                            break;
                    }
                }

                return stack;
            }).Single();
        }
    }
}
