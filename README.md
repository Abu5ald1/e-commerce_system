# E-commerce System Documentation

## Overview
This document explains how I approached and solved the e-commerce system assignment. The system handles product management, shopping cart functionality, and checkout processing with support for perishable and shippable items.

## Business Requirements Analysis

### Core Features Required:
1. **Product Management**: Define products with name, price, and quantity
2. **Expiration Handling**: Some products expire (Cheese, Biscuits) while others don't (TV, Mobile)
3. **Shipping Management**: Some products require shipping with weight tracking
4. **Shopping Cart**: Add products with quantity validation
5. **Checkout Process**: Calculate totals, handle payments, and process shipments

## Solution Architecture

### Class Design Approach

I used an object-oriented approach with clear separation of concerns:

#### 1. **Product Class (Base Class)**
```csharp
internal class Product
{
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public DateTime? ExpirationDate { get; set; }
}
```

**Design Decision**: Used nullable DateTime for expiration to handle both perishable and non-perishable products elegantly.

#### 2. **IShipping Interface**
```csharp
internal interface IShipping
{
    string getName();
    double getWeight();
}
```

**Design Decision**: Created a separate interface following the assignment requirements. This allows any product to be shippable by implementing this interface.

#### 3. **ShippableProduct Class**
```csharp
internal class ShippableProduct : Product, IShipping
{
    public float Weight { get; set; }
}
```

**Design Decision**: Used inheritance to extend Product functionality while implementing IShipping interface. This avoids code duplication.

#### 4. **Cart Class**
```csharp
internal class Cart
{
    private Dictionary<Product, int> items = new Dictionary<Product, int>();
}
```

**Design Decision**: Used Dictionary to store products and their quantities efficiently. This prevents duplicate products and makes quantity management easier.

#### 5. **Customer Class**
```csharp
internal class Customer
{
    public string Name { get; set; }
    public double Balance { get; set; }
    public Cart Cart { get; set; }
}
```

**Design Decision**: Simple class that owns a cart and has a balance for payment processing.

## Key Implementation Challenges & Solutions

### Challenge 1: Stock Management
**Problem**: How to handle stock reduction when adding to cart vs. during checkout?

**Solution**: I decided to reduce stock immediately when adding to cart. This prevents overselling and makes the system more realistic.

```csharp
public void AddProduct(Product product, int amount)
{
    if (amount <= product.Quantity && amount > 0)
    {
        // Add to cart
        if (items.ContainsKey(product))
            items[product] += amount;
        else
            items.Add(product, amount);
        
        // Reduce stock immediately
        product.Quantity -= amount;
    }
}
```

### Challenge 2: Shipping Collection
**Problem**: How to collect only shippable items for the shipping service?

**Solution**: Used type checking with `is` operator to identify shippable products and collect them in a separate dictionary.

```csharp
if (product is IShipping shippable)
{
    if (shippedProducts.ContainsKey(shippable))
        shippedProducts[shippable] += amount;
    else
        shippedProducts[shippable] = amount;
}
```

### Challenge 3: Expiration Validation
**Problem**: When to check for expired products?

**Solution**: I implemented checks in two places:
1. **During cart addition**: Prevents adding expired products
2. **During checkout**: Final validation before payment

```csharp
public bool IsExpired()
{
    return ExpirationDate != null && ExpirationDate < DateTime.Now;
}
```

### Challenge 4: Error Handling
**Problem**: Multiple validation points during checkout.

**Solution**: Used early return pattern to exit immediately when validation fails:

```csharp
public static void CheckOut(Customer customer)
{
    // Empty cart check
    if (customer.Cart.GetCartItems().Count == 0)
    {
        Console.WriteLine("Cart is empty!");
        return;
    }
    
    // Expiration check
    if (product.IsExpired())
    {
        Console.WriteLine($"{product.Name} is Expired!");
        return;
    }
    
    // Balance check
    if (customer.Balance < subtotal + shippingCost)
    {
        Console.WriteLine("Balance Isn't sufficient!");
        return;
    }
}
```

## Business Logic Decisions

### 1. **Shipping Cost Calculation**
I used a rate of 0.14 per gram (14 cents per gram):
```csharp
shippingCost = shippedProducts.Sum(p => p.Key.getWeight() * p.Value) * 0.14;
```

### 2. **Payment Processing**
Balance is deducted only after all validations pass:
```csharp
customer.Balance -= (subtotal + shippingCost);
```

### 3. **Cart Restoration**
When removing items or clearing cart, stock is restored:
```csharp
public void ClearCart()
{
    foreach (var item in items)
    {
        item.Key.Quantity += item.Value;
    }
    items.Clear();
}
```

## Testing Strategy

### Test Case 1: Normal Purchase
```csharp
customer1.Cart.AddProduct(cheese, 3);
customer1.Cart.AddProduct(biscuits, 5);
customer1.Cart.AddProduct(mobileScratch, 2);
CheckOut(customer1);
```
**Expected**: Successful checkout with shipping notice and receipt.

## Key Assumptions Made

1. **Stock Management**: Stock is reduced immediately when adding to cart
2. **Shipping Rate**: Fixed rate of 0.14 per gram
3. **Weight Units**: All weights are in grams
4. **Currency**: All prices are in a single currency unit
5. **Expiration Check**: Products expire at the exact DateTime specified
6. **Balance Format**: Customer balance is stored as double for simplicity

## Output Format

The system produces two types of output:

### 1. Shipment Notice
```
** Shipment notice **
3X Cheese
750g
5X Biscuits
150g
Total package weight 0.9kg
```

### 2. Checkout Receipt
```
** Checkout receipt **
3X  Cheese      360
5X  Biscuits    25
2X  Mobile Scratch  40
----------------------
Subtotal         425
Shipping         126
Total Paid Amount    551
Current balance      6949
```

## Conclusion

This solution successfully addresses all the business requirements while maintaining clean, readable code. The object-oriented design allows for easy extension and modification. The use of interfaces and inheritance provides flexibility for future enhancements like different shipping methods or product types.

The system handles edge cases gracefully and provides clear feedback to users about why operations might fail. The separation of concerns makes the code maintainable and testable.
