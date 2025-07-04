using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce_system
{
    // handles shipping of products
    internal class ShippingService
    {
        public static void ShipProducts(Dictionary<IShipping, int> shippedProducts)
        {
            Console.WriteLine("** Shipment notice **");
            double TotalWieght = 0;

            // Display each item being shipped
            foreach (var pair in shippedProducts)
            {
                var item = pair.Key;
                var amount = pair.Value;
                var totalItemWeight = item.getWeight() * amount;

                Console.WriteLine($"{amount}X {item.getName()}");
                Console.WriteLine($"{totalItemWeight}g");

                TotalWieght += totalItemWeight;
            }
            // Show the total package weight
            Console.WriteLine($"Total package weight {TotalWieght/1000.0}kg\n");
        }
    }
}
