using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce_system
{
    //The Shopping Cart
    internal class Cart
    {
        private Dictionary<Product, int> Items = new Dictionary<Product, int>();

        public void AddProduct(Product P, int amount)
        {
            // Don't Add Expired Products
            if (P.IsExpired())
            {
                Console.WriteLine("The Product Is Expired!");
                return;
            }

            // Check the Stock
            if (amount <= P.Quantity && amount >0)
            {
                if (Items.ContainsKey(P))
                    Items[P] += amount;
                else
                    Items.Add(P, amount);

                // Reduce The Stock
                P.Quantity -= amount;
            }
            else
            {
                Console.WriteLine($"Not Enough Amount for {P.Name}");
            }
        }

        public void DeleteProduct(Product P)
        {
            if (Items.ContainsKey(P))
            {
                // Return amount to stock
                P.Quantity += Items[P];
                Items.Remove(P);
            }
            else
            {
                Console.WriteLine("Product is not found in cart!");
            }
        }


        // Empty Cart and restore all amounts to stock
        public void ClearCart()
        {
            foreach (var item in Items)
            {
                item.Key.Quantity += item.Value;
            }
            Items.Clear();
        }

        // Get total Price of all items in cart
        public double GetSubTotal()
        {
            double subTotal = 0;
            foreach (var pair in Items)
            {
                subTotal += (pair.Value * pair.Key.Price);
            }
            return subTotal;
        }

        // Get all items in cart
        public Dictionary<Product, int> GetCartItems()
        {
            return Items;
        }

    }
}
