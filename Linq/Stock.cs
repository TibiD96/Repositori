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

            CallBackNotifications(list[ProductIndex(name)], quantity);

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

        private void ProductQuantityInDepo(string name, int quantity)
        {
            if (list[ProductIndex(name)].Quantity >= quantity)
            {
                return;
            }

            throw new ArgumentException("Not enough quantity");
        }

        private void CallBackNotifications(Product product, int quanityToSell)
        {
            int[] notifQuant = new[] { 10, 5, 2, 0 };
            int notiVal = notifQuant.Where(notifValues => notifValues <= product.Quantity)
                                   .FirstOrDefault(newNotifQuant => product.Quantity - quanityToSell < newNotifQuant);
            if (product.Quantity - quanityToSell > notiVal)
            {
                return;
            }

            Notifications(product);
        }

        internal class Product
        {
            public string Name { get; set; }

            public int Quantity { get; set; }
        }
    }
}
