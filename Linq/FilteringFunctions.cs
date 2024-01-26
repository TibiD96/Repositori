using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Linq
{
    public class FilteringFunctions
    {
        public static IEnumerable<ProductWithFeature> AtLeastOneFeature(List<ProductWithFeature> inputListProd, List<Feature> inputListFeature)
        {
            return inputListProd.Where(prod => prod.Features.Intersect(inputListFeature).Any());
        }

        public static IEnumerable<ProductWithFeature> AllFeatures(List<ProductWithFeature> inputListProd, List<Feature> inputListFeature)
        {
            return inputListProd.Where(prod => !inputListFeature.Except(prod.Features).Any());
        }

        public static IEnumerable<ProductWithFeature> NoFeature(List<ProductWithFeature> inputListProd, List<Feature> inputListFeature)
        {
            return inputListProd.Where(prod => !prod.Features.Intersect(inputListFeature).Any());
        }

        public static IEnumerable<Product> ProductQuantity(List<Product> firstInputListProd, List<Product> secondInputListProd)
        {
            return firstInputListProd.Concat(secondInputListProd).GroupBy(product => product.Name)
                                     .Select(prod => new Product(prod.Key, prod.Sum(prodQuant => prodQuant.Quantity)));
        }

        public static IEnumerable<TestResults> RankingResult(List<TestResults> inputRanking)
        {
            return inputRanking.GroupBy(fam => fam.FamilyId).Select(group => group.MaxBy(score => score.Score));
        }

        public static IEnumerable<(int, string)> WordRanking(string inputString)
        {
            const string delimitations = " ,.!?:\"";
            return inputString.ToLower().Split(delimitations.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                        .GroupBy(words => words)
                                        .Select(group => (Count: group.Count(), Word: group.Key))
                                        .OrderByDescending(pair => pair.Count);
        }

        public static bool SudokuValidator(int[,] sudokuTable)
        {
            var rows = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9)
                                             .Select(j => sudokuTable[i, j]));

            var column = Enumerable.Range(0, 9).Select(j => Enumerable.Range(0, 9)
                                               .Select(i => sudokuTable[i, j]));

            var blocks = Enumerable.Range(0, 3).SelectMany(i => Enumerable.Range(0, 3)
                                               .Select(j => Enumerable.Range(0, 3)
                                               .SelectMany(k => Enumerable.Range(0, 3)
                                               .Select(l => sudokuTable[3 * i + k, 3 * j + l]))));

            bool GroupIsValid(IEnumerable<int> group)
            {
                return group.GroupBy(element => element).All(g => g.Count() == 1 && g.Key >= 1 && g.Key <= 9);
            }

            return rows.Concat(column).Concat(blocks).All(GroupIsValid);
        }

        public static double PostfixEquation(string inputEquation)
        {
            string[] arrayOfElements = inputEquation.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<double> stack = new List<double>();

            double Calculus(double firstNumber, double secondNumber, string operand)
            {
                return operand switch
                {
                    "+" => firstNumber + secondNumber,
                    "-" => firstNumber - secondNumber,
                    "*" => firstNumber * secondNumber,
                    "/" => firstNumber / secondNumber,
                };
            }

            return arrayOfElements.Aggregate(stack, (stack, operand) =>
            {
                if (double.TryParse(operand, out double number))
                {
                    stack.Add(number);
                }
                else
                {
                    double firstNumber = stack[^1];
                    double secondNumber = stack[^2];

                    stack.RemoveRange(stack.Count - 2, 2);

                    stack.Add(Calculus(firstNumber, secondNumber, operand));
                }

                return stack;
            }).Single();
        }
    }
}
