using System.Security.Cryptography.X509Certificates;

namespace e_commerce_system
{
    internal class Program
    {
        static void Main(string[] args)
        {

            ShippableProduct Cheese = new ShippableProduct("Cheese", 120, 10, DateTime.Now.AddDays(7), 250);
            ShippableProduct TV = new ShippableProduct("Tv", 10000, 5, null, 10000);
            ShippableProduct Mobile = new ShippableProduct("Mobile", 25000, 5, null, 200);
            ShippableProduct Biscuits = new ShippableProduct("Biscuits", 5, 17, DateTime.Now.AddDays(90), 30);
            Product MobileScratch = new Product("Mobile Scratch", 20, 10, DateTime.Now.AddMonths(12));

            Customer customer1 = new Customer("Ahmed", 7500);


            customer1.Cart.AddProduct(Cheese, 3);
            customer1.Cart.AddProduct(Biscuits, 5);
            customer1.Cart.AddProduct(MobileScratch, 2);

            CheckOut(customer1);


        }


        public static void CheckOut(Customer customer)
        {
            Dictionary<IShipping,int> ShippedProducts = new Dictionary<IShipping,int>();

            double ShippingCost = 0;
            double subtotal = customer.Cart.GetSubTotal();

            // Check if cart is empty
            if (customer.Cart.GetCartItems().Count == 0)
            {
                Console.WriteLine("Cart is empty!");
                return;
            }

            // Proccess each item in cart (Check for Expiration, Stock, Calculate Shipping)
            foreach (var pair in customer.Cart.GetCartItems())
            {
                var product = pair.Key;
                var amount = pair.Value;
                if (product.IsExpired())
                {
                    Console.WriteLine($"{product.Name} is Expired!");
                    return;
                }

                // Collect Items that need shipping
                if (product is IShipping shippable)
                {
                    if (ShippedProducts.ContainsKey(shippable))
                        ShippedProducts[shippable] += amount;
                    else
                        ShippedProducts[shippable] = amount;
                }
            }

            // Calculate shipping Cost
            ShippingCost = ShippedProducts.Sum(p => p.Key.getWeight() * p.Value) * 0.14;

            // Check if customer has enough balance
            if (customer.Balance < subtotal + ShippingCost)
            {
                Console.WriteLine("Balance Isn't sufficient!"); 
                return;
            }

            // Proccess Shipping if existing
            if (ShippedProducts.Any())
            {
                ShippingService.ShipProducts(ShippedProducts);
            }

            // Print the receipt
            Console.WriteLine("** Checkout receipt **\n");
            foreach (var pair in customer.Cart.GetCartItems())
            {
                var product = pair.Key;
                var amount = pair.Value;
                Console.WriteLine($"{amount}X  {product.Name}\t\t{(amount * product.Price)}\n");
            }

            // Print Total
            Console.WriteLine
            (
                $"----------------------\nSubtotal\t\t {subtotal}" +
                $"\nShipping\t\t {ShippingCost}\n" +
                $"Total Paid Amount\t\t {subtotal + ShippingCost}\n" +
                $"Current balance\t\t {customer.Balance - (subtotal + ShippingCost)}"
            );

            // Reduce money from cutomer balance
            customer.Balance -= (subtotal + ShippingCost);
            
        }
    }
}
