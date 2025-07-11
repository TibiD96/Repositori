﻿using System.Linq;
using Xunit;

namespace Linq
{
    public class LinqMethodesTests
    {
        [Fact]

        public void CheckAllMethode()
        {
            var first = new int[4];
            first[0] = 1;
            first[1] = 3;
            first[2] = 5;
            first[3] = 7;

            var second = new int[4];
            second[0] = 2;
            second[1] = 4;
            second[2] = 6;
            second[3] = 8;

            Assert.True(first.All(x => x % 2 == 1));
            Assert.False(second.All(x => x % 2 == 1));
            Assert.Throws<ArgumentNullException>(() => LinqMethodes.All<int>(null, x => x % 2 == 1));
        }

        [Fact]

        public void CheckAnyMethode()
        {
            var first = new int[4];
            first[0] = 2;
            first[1] = 3;
            first[2] = 4;
            first[3] = 8;

            var second = new int[4];
            second[0] = 2;
            second[1] = 4;
            second[2] = 6;
            second[3] = 8;

            Assert.True(first.Any(x => x % 2 == 1));
            Assert.False(second.Any(x => x % 2 == 1));
            Assert.Throws<ArgumentNullException>(() => LinqMethodes.Any<int>(null, x => x % 2 == 1));
            Assert.Throws<ArgumentNullException>(() => first.Any(null));
        }

        [Fact]

        public void CheckFirstMethode()
        {
            var first = new int[4];
            first[0] = 2;
            first[1] = 3;
            first[2] = 4;
            first[3] = 8;

            var second = new int[4];
            second[0] = 2;
            second[1] = 4;
            second[2] = 6;
            second[3] = 8;

            Assert.Equal(first[1], first.First(x => x % 2 == 1));
            Assert.Throws<InvalidOperationException>(() => second.First(x => x % 2 == 1));
            Assert.Throws<ArgumentNullException>(() => LinqMethodes.First<int>(null, x => x % 2 == 1));
            Assert.Throws<ArgumentNullException>(() => first.First(null));
        }

        [Fact]

        public void CheckSelectMethode()
        {
            var first = new int[4];
            first[0] = 2;
            first[1] = 3;
            first[2] = 4;
            first[3] = 8;

            var final = new int[4];
            final[0] = 4;
            final[1] = 9;
            final[2] = 16;
            final[3] = 64;

            Assert.Equal(final, first.Select(x => x * x));
        }

        [Fact]

        public void CheckSelectManyMethode()
        {
            var first = new List<int[]>
            {
                new[]
                {
                    1, 2, 3, 4
                },

                new[]
                {
                    5, 6, 7, 8
                },

                new[]
                {
                    9, 10, 11, 12
                }
            };

            var final = new string[12];
            final[0] = "1";
            final[1] = "2";
            final[2] = "3";
            final[3] = "4";
            final[4] = "5";
            final[5] = "6";
            final[6] = "7";
            final[7] = "8";
            final[8] = "9";
            final[9] = "10";
            final[10] = "11";
            final[11] = "12";

            var result = first.SelectMany(x => x.Select(y => y.ToString()));
            Assert.Equal(final, result);
        }

        [Fact]

        public void CheckWhereMethode()
        {
            var first = new int[4];
            first[0] = 2;
            first[1] = 3;
            first[2] = 4;
            first[3] = 7;

            var final = new int[2];
            final[0] = 2;
            final[1] = 4;
            var result = first.Where(x => x % 2 == 0);
            Assert.Equal(final, result);
        }

        [Fact]

        public void CheckToDictionaryMethode()
        {
            var workers = new List<Employes>
            {
               new Employes { Name = "Andre", Age = 20 },

               new Employes { Name = "Cristi", Age = 40 },

               new Employes { Name = "Ana", Age = 25 },

               new Employes { Name = "Ilie", Age = 50 }
            };

            var dictionar = workers.ToDictionary(employes => employes.Name, employes => employes.Age);

            Assert.Equal(50, dictionar["Ilie"]);
            Assert.Equal(25, dictionar["Ana"]);
            Assert.Equal(40, dictionar["Cristi"]);
            Assert.Equal(20, dictionar["Andre"]);
        }

        [Fact]

        public void CheckZipMethode()
        {
            var workers = new[]
            {
                "Alex", "Maria", "Cristina", "Bogdan"
            };

            var salary = new[]
            {
               2000, 3000, 1500
            };

            var zip = workers.Zip(salary, (first, second) => first + " " + second);

            var result = new[]
            {
                "Alex 2000", "Maria 3000", "Cristina 1500"
            };

            Assert.Equal(result, zip);
        }

        [Fact]

        public void CheckAgregateMethode()
        {
            var workers = new[]
            {
                "Ion", "Maria", "Cristina", "Adi"
            };

            var agregate = workers.Aggregate("Mihai", (longest, next) => next.Length > longest.Length ? next : longest);

            const string result = "Cristina";

            Assert.Equal(result, agregate);
        }

        [Fact]

        public void CheckJoinMethode()
        {
            Employes andre = new Employes { Name = "Andre", Age = 20 };
            Employes cristi = new Employes { Name = "Cristi", Age = 20 };
            Employes ana = new Employes { Name = "Ana", Age = 20 };
            Employes ilie = new Employes { Name = "Ilie", Age = 20 };

            Rise tenPercent = new Rise { Name = "Andre", Amount = "10%" };
            Rise fifteenPercent = new Rise { Name = "Cristi", Amount = "15%" };
            Rise twentyPercent = new Rise { Name = "Ana", Amount = "20%" };
            Rise fivePercent = new Rise { Name = "Ilie", Amount = "5%" };

            List<Employes> workers = new List<Employes> { andre, cristi, ana, ilie };
            List<Rise> bonus = new List<Rise> { tenPercent, fifteenPercent, twentyPercent, fivePercent };

            var join = workers.Join(bonus, employes => employes.Name, rise => rise.Name, (employes, bonus) => employes.Name + " " + bonus.Amount);
            var result = new List<string> { "Andre 10%", "Cristi 15%", "Ana 20%", "Ilie 5%" };
            Assert.Equal(result, join);
        }

        [Fact]

        public void CheckToDistincMethode()
        {
            var workers = new[]
            {
               "ana", "andrei", "ana", "alin", "andrei", "cristi", "marian", "cristi"
            };

            var result = new[]
            {
               "ana", "andrei", "alin", "cristi", "marian"
            };

            var distinct = LinqMethodes.Distinct(workers, EqualityComparer<string>.Default);

            Assert.Equal(result, distinct);
        }

        [Fact]

        public void CheckToUnionMethode()
        {
            var first = new[]
            {
               "ana", "maria", "claudia", "laura", "cristi"
            };

            var second = new[]
            {
               "mihai", "cristi", "ionut", "daniel", "maria"
            };

            var result = new[]
            {
               "ana", "maria", "claudia", "laura", "cristi", "mihai", "ionut", "daniel"
            };

            var union = LinqMethodes.Union(first, second, EqualityComparer<string>.Default);

            Assert.Equal(result, union);
        }

        [Fact]

        public void CheckToIntersectMethode()
        {
            var first = new[]
            {
               "ana", "maria", "claudia", "laura", "cristi"
            };

            var second = new[]
            {
               "mihai", "cristi", "ionut", "daniel", "maria"
            };

            var result = new[]
            {
               "maria", "cristi"
            };

            var intersect = LinqMethodes.Intersect(first, second, EqualityComparer<string>.Default);

            Assert.Equal(result, intersect);
        }

        [Fact]

        public void CheckToExceptMethode()
        {
            var first = new[]
            {
               "ana", "maria", "claudia", "laura", "cristi"
            };

            var second = new[]
            {
               "mihai", "cristi", "ionut", "daniel", "maria"
            };

            var result = new[]
            {
               "ana", "claudia", "laura"
            };

            var except = LinqMethodes.Expect(first, second, EqualityComparer<string>.Default);

            Assert.Equal(result, except);
        }

        [Fact]

        public void CheckToGroupByMethodeWithInt()
        {
            var numbers = new[]
            {
               1, 2, 3, 4, 5, 6
            };

            var groupBy = LinqMethodes.GroupBy(numbers, x => x % 2 == 0 ? "Even" : "Odd", x => x, (key, x) => new { Key = key, Group = x.ToList() }, EqualityComparer<string>.Default);

            var evenNumbers = groupBy.FirstOrDefault(x => x.Key == "Even");
            var oddNumbers = groupBy.FirstOrDefault(x => x.Key == "Odd");

            Assert.Equal(new List<int> { 2, 4, 6 }, evenNumbers.Group);
            Assert.Equal(new List<int> { 1, 3, 5 }, oddNumbers.Group);
        }

        [Fact]

        public void CheckToGroupByMethodeWithString()
        {
            var names = new[]
            {
               "ana", "maria", "ion", "mirel", "anastasia", "dia", "mincu"
            };

            var groupBy = LinqMethodes.GroupBy(names, names => names.Length, names => names, (key, x) => new { Key = key, Group = x.ToList() }, EqualityComparer<int>.Default);

            var three = groupBy.FirstOrDefault(names => names.Key == 3);
            var five = groupBy.FirstOrDefault(names => names.Key == 5);
            var nine = groupBy.FirstOrDefault(names => names.Key == 9);

            Assert.Equal(new List<string> { "ana", "ion", "dia" }, three.Group);
        }

        [Fact]

        public void CheckToOrderByMethodeWithString()
        {
            var names = new[]
            {
               "andu", "anastasia", "dia", "mincu"
            };

            var orderBy = LinqMethodes.OrderBy(names, names => names.Length, Comparer<int>.Default);

            var result = new[]
            {
               "dia", "andu", "mincu", "anastasia"
            };

            Assert.Equal(result, orderBy);
        }

        [Fact]

        public void CheckToOrderByMethodeWithInt()
        {
            var numbers = new[]
            {
               5, 3, 20, 6
            };

            var orderBy = LinqMethodes.OrderBy(numbers, x => x, Comparer<int>.Default);

            var result = new[]
            {
               3, 5, 6, 20
            };

            Assert.Equal(result, orderBy);
        }

        [Fact]

        public void CheckToOrderByMethodeWithList()
        {
            var workers = new List<Employes>
            {
               new Employes { Name = "Andre", Age = 20 },

               new Employes { Name = "Cristi", Age = 40 },

               new Employes { Name = "Ana", Age = 25 },

               new Employes { Name = "Ilie", Age = 50 }
            };

            var orderBy = LinqMethodes.OrderBy(workers, employes => employes.Age, Comparer<int>.Default);

            var result = new List<Employes>
            {
               new Employes { Name = "Andre", Age = 20 },

               new Employes { Name = "Ana", Age = 25 },

               new Employes { Name = "Cristi", Age = 40 },

               new Employes { Name = "Ilie", Age = 50 }
            };

            Assert.Equal(result, orderBy);
        }

        [Fact]

        public void CheckToThenByMethodeWithList()
        {
            var workers = new List<Employes>
            {
               new Employes { Name = "Andre", Age = 20 },

               new Employes { Name = "Cristi", Age = 40 },

               new Employes { Name = "Ana", Age = 25 },

               new Employes { Name = "Ilie", Age = 50 }
            };

            var thenBy = workers.OrderBy(employes => employes.Age, Comparer<int>.Default).ThenBy(employes => employes.Name.Length, Comparer<int>.Default);

            var result = new List<Employes>
            {
               new Employes { Name = "Andre", Age = 20 },

               new Employes { Name = "Ana", Age = 25 },

               new Employes { Name = "Cristi", Age = 40 },

               new Employes { Name = "Ilie", Age = 50 }

            };

            Assert.Equal(result, thenBy);
        }

        [Fact]

        public void CheckToThenByMethodeWithListMultipleThenBy()
        {
            var workers = new List<Employes>
            {
               new Employes { Name = "Andre", Age = 20 },

               new Employes { Name = "Cristi", Age = 40 },

               new Employes { Name = "Ana", Age = 25 },

               new Employes { Name = "Ilie", Age = 50 }
            };

            var thenBy = LinqMethodes.OrderBy(workers, employes => employes.Age, Comparer<int>.Default).ThenBy(employes => employes.Age, Comparer<int>.Default).ThenBy(employes => employes.Name.Length, Comparer<int>.Default);

            var result = new List<Employes>
            {
               new Employes { Name = "Andre", Age = 20 },

               new Employes { Name = "Ana", Age = 25 },

               new Employes { Name = "Cristi", Age = 40 },

               new Employes { Name = "Ilie", Age = 50 }

            };

            Assert.Equal(result, thenBy);
        }

        [Fact]

        public void CheckToThenByMethodeWithListMultipleThenByWithDifferentKey()
        {
            var workers = new List<Employes>
            {
               new Employes { Name = "Andre", Age = 20 },

               new Employes { Name = "Cristi", Age = 40 },

               new Employes { Name = "Bna", Age = 12 },

               new Employes { Name = "Ilie", Age = 50 }
            };

            var thenBy = LinqMethodes.OrderBy(workers, employes => employes.Age, Comparer<int>.Default).ThenBy(employes => employes.Name, Comparer<string>.Default);

            var result = new List<Employes>
            {
               new Employes { Name = "Bna", Age = 12 },

               new Employes { Name = "Andre", Age = 20 },

               new Employes { Name = "Cristi", Age = 40 },

               new Employes { Name = "Ilie", Age = 50 }
            };

            Assert.Equal(result, thenBy);
        }

        [Fact]

        public void CheckComparerChooser()
        {
            var workers = new List<Employes>
            {
               new Employes { Name = "Andre", Age = 12 },

               new Employes { Name = "Cristi", Age = 40 },

               new Employes { Name = "Bna", Age = 2 },

               new Employes { Name = "Ilie", Age = 50 }
            };

            var thenBy = LinqMethodes.OrderBy(workers, employes => employes.Name, Comparer<string>.Default);

            var result = new List<Employes>
            {
               new Employes { Name = "Andre", Age = 12 },

               new Employes { Name = "Bna", Age = 2 },

               new Employes { Name = "Cristi", Age = 40 },

               new Employes { Name = "Ilie", Age = 50 }
            };

            Assert.Equal(result, thenBy);
        }

        [Fact]

        public void CheckCreateOrderedEnumerable()
        {
            string[] fruits =
            {
                "grape", "passionfruit", "banana", "mango",
                "orange", "raspberry", "apple", "blueberry"
            };

            var final = LinqMethodes.OrderBy(fruits, fruits => fruits.Length, Comparer<int>.Default).ThenBy(fruits => fruits, Comparer<string>.Default);

            string[] result =
            {
                "apple", "grape", "mango", "banana",
                "orange", "blueberry", "raspberry", "passionfruit"
            };

            Assert.Equal(result, final);
        }

        [Fact]

        public void CreateOrderedEnumerable()
        {
            var workers = new List<Employes>
            {
               new Employes { Name = "A", Age = 12 },

               new Employes { Name = "C", Age = 40 },

               new Employes { Name = "A", Age = 20 },

               new Employes { Name = "I", Age = 50 },

               new Employes { Name = "C", Age = 91 },

               new Employes { Name = "A", Age = 50 },

               new Employes { Name = "I", Age = 15 },

               new Employes { Name = "C", Age = 32 }
            };

            var thenBy = LinqMethodes.OrderBy(workers, employes => employes.Name, Comparer<string>.Default).ThenBy(employes => employes.Age, Comparer<int>.Default);

            var result = new List<Employes>
            {
               new Employes { Name = "A", Age = 12 },

               new Employes { Name = "A", Age = 20 },

               new Employes { Name = "A", Age = 50 },

               new Employes { Name = "C", Age = 32 },

               new Employes { Name = "C", Age = 40 },

               new Employes { Name = "C", Age = 91 },

               new Employes { Name = "I", Age = 15 },

               new Employes { Name = "I", Age = 50 }
            };

            Assert.Equal(result, thenBy);
        }

        private class Employes
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public override bool Equals(object obj)
            {
                return obj is Employes other && Name == other.Name && Age == other.Age;
            }
        }

        private class Rise
        {
            public string Name { get; set; }

            public string Amount { get; set; }
        }
    }
}