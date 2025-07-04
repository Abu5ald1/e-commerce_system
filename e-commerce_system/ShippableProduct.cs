using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace e_commerce_system
{
    // The Products That needs to be shipped
    internal class ShippableProduct: Product, IShipping
    {
        public float Weight { get; set;}

        public ShippableProduct(string Name, double Price,int Quantity, DateTime? ExpirationDate, float weight) : base(Name, Price, Quantity, ExpirationDate)
        {
            Weight = weight;
        }


        // IShipping Interface Methods
        public string getName() => Name;
        public double getWeight() => Weight;
    }
}
