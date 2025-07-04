using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce_system
{
    internal class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? ExpirationDate{ get; set; }

        public Product(string name, double price, int quantity, DateTime? expirationDate= null)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
            ExpirationDate = expirationDate;
        }

        //Check if Product is expired
        public bool IsExpired()
        {
            return ExpirationDate != null && ExpirationDate < DateTime.Now;
        }
    }

}
