using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq
{
    public class Stock
    {
        internal Action<Product> Notifications;
        private readonly List<Product> list;

        public Stock()
        {
            list = new List<Product>();
        }

        public int Count
        {
            get { return list.Count; }
        }

        internal List<Product> Products
        {
            get { return list; }
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

            ProductQuantityInDepo(name, quantity);

            list[ProductIndex(name)].Quantity = list[ProductIndex(name)].Quantity - quantity;

            CallBackNotifications(list[ProductIndex(name)]);
        }

        public bool Find(string name)
        {
            return list.Any(x => x.Name == name);
        }

        public int ProductIndex(string name)
        {
            return list.FindIndex(0, list.Count, product => product.Name == name);
        }

        private void ProductQuantityInDepo(string name, int quantity)
        {
            if (list[ProductIndex(name)].Quantity >= quantity)
            {
                return;
            }

            throw new ArgumentException("Not enough quantity");
        }

        private void CallBackNotifications(Product product)
        {
            int[] notifQuant = new[] { 2, 5, 10 };
            foreach (var quantity in notifQuant.Where(quantity => product.Quantity < quantity))
            {
                Notifications(product);
            }
        }

        internal class Product
        {
            public string Name { get; set; }

            public int Quantity { get; set; }
        }
    }
}
