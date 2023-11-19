using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class Stock
    {
        private readonly List<Product> list;

        public Stock()
        {
            list = new List<Product>();
        }

        public int Count
        {
            get { return list.Count; }
        }

        public void Add(string name, int quantity)
        {
            list.Add(new Product { Name = name, Quantity = quantity });
        }

        public bool Find(string name)
        {
            return list.Any(x => x.Name == name);
        }

        private class Product
        {
            public string Name { get; set; }

            public int Quantity { get; set; }
        }
    }
}
