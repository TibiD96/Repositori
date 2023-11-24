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
            if (Find(name))
            {
                throw new ArgumentException("Product allready exist");
            }

            list.Add(new Product { Name = name, Quantity = quantity });
        }

        public void ProductOrder(string name, int quantity)
        {
            if (!Find(name))
            {
                throw new ArgumentException("Product don't exist");
            }

            ProductQuantitieInDepo(name, quantity);

            list[ProductIndex(name)].Quantity = list[ProductIndex(name)].Quantity - quantity;
        }

        public bool Find(string name)
        {
            return list.Any(x => x.Name == name);
        }

        public int ProductIndex(string name)
        {
            return list.FindIndex(0, list.Count, product => product.Name == name);
        }

        private void ProductQuantitieInDepo(string name, int quantity)
        {
            if (list[ProductIndex(name)].Quantity >= quantity)
            {
                return;
            }

            throw new ArgumentException("Not enough quantity");
        }

        internal class Product
        {
            public string Name { get; set; }

            public int Quantity { get; set; }
        }
    }
}
