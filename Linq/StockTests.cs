using System;
using Linq;
using Xunit;

namespace Linq
{
    public class StockTests
    {
        [Fact]

        public void CheckAdd()
        {
            var stock = new Stock();
            stock.Add("BMW", 1000);
            stock.Add("Mercedes", 300);
            stock.Add("Audi", 500);

            Assert.Equal(3, stock.Count);
        }

        [Fact]

        public void CheckFind()
        {
            var stock = new Stock();
            stock.Add("BMW", 1000);
            stock.Add("Mercedes", 300);
            stock.Add("Audi", 500);

            Assert.True(stock.Find("Mercedes"));
        }

        [Fact]

        public void CheckProductIndex()
        {
            var stock = new Stock();
            stock.Add("BMW", 1000);
            stock.Add("Mercedes", 300);
            stock.Add("Audi", 500);

            var result = stock.ProductIndex("Mercedes");

            Assert.Equal(1, result);
        }

        [Fact]

        public void CheckProductOrder()
        {
            var stock = new Stock();
            stock.Add("BMW", 1000);
            stock.Add("Mercedes", 300);
            stock.Add("Audi", 500);

            Assert.Throws<ArgumentException>(() => stock.ProductOrder("BMW", 1500));
        }
    }
}
