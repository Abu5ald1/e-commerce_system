using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce_system
{
    internal class Customer
    {
        public string Name { get; set; }
        public double Balance { get; set; }
        public Cart Cart { get; set; }
        public Customer(string name, double balance)
        {
            Name = name;
            Balance = balance;
            Cart = new Cart();
        }

    }
}
