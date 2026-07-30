using System.Text.Json.Serialization;
using RestaurantOrderSystem.Enums;


namespace RestaurantOrderSystem.Models;

public class OrderItem
{
    public int MenuItemId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    [JsonInclude]
    public int Quantity { get; private set; }

    public decimal LineTotal => UnitPrice * Quantity;

    public OrderItem()
    {
    }

    public OrderItem(
        MenuItem menuItem,
        int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.");
        }

        MenuItemId = menuItem.Id;
        Name = menuItem.Name;
        UnitPrice = menuItem.Price;
        Quantity = quantity;
    }

    public void AddQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.");
        }

        Quantity += quantity;
    }
}

public class Order
{
    private const decimal TaxRate = 0.08m;
    private const decimal DeliveryFee = 5.00m;
    private const decimal ServiceRate = 0.10m;

    public int OrderId { get; set; }

    public string CustomerName { get; set; }
        = string.Empty;

    public OrderType Type { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.Now;

    [JsonInclude]
    public List<OrderItem> Items { get; private set; }
        = new();

    public Order()
    {
    }

    public Order(
        int orderId,
        string customerName,
        OrderType type)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new ArgumentException(
                "Customer name cannot be empty.");
        }

        OrderId = orderId;
        CustomerName = customerName;
        Type = type;
    }

    public void AddItem(
        MenuItem menuItem,
        int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.");
        }

        OrderItem? existingItem = Items.FirstOrDefault(
            item => item.MenuItemId == menuItem.Id);

        if (existingItem != null)
        {
            existingItem.AddQuantity(quantity);
        }
        else
        {
            Items.Add(
                new OrderItem(menuItem, quantity));
        }
    }

    public decimal CalculateSubtotal()
    {
        return Items.Sum(item => item.LineTotal);
    }

    public decimal CalculateTax()
    {
        return CalculateSubtotal() * TaxRate;
    }

    public decimal CalculateExtraFee()
    {
        return Type switch
        {
            OrderType.DineIn =>
                CalculateSubtotal() * ServiceRate,

            OrderType.Delivery =>
                DeliveryFee,

            _ => 0m
        };
    }

    public decimal CalculateTotal()
    {
        return CalculateSubtotal()
               + CalculateTax()
               + CalculateExtraFee();
    }

    public void CompleteOrder()
    {
        if (Items.Count == 0)
        {
            throw new EmptyOrderException();
        }
    }

    public void DisplayOrder()
    {
        Console.WriteLine();
        Console.WriteLine("===== CURRENT ORDER =====");
        Console.WriteLine($"Order ID: {OrderId}");
        Console.WriteLine($"Customer: {CustomerName}");
        Console.WriteLine($"Type: {Type}");
        Console.WriteLine($"Created: {CreatedAt:g}");

        if (Items.Count == 0)
        {
            Console.WriteLine("The order is empty.");
        }
        else
        {
            foreach (OrderItem item in Items)
            {
                Console.WriteLine(
                    $"{item.Name} x{item.Quantity} " +
                    $"= ${item.LineTotal:F2}");
            }
        }

        Console.WriteLine(
            $"Subtotal: ${CalculateSubtotal():F2}");

        Console.WriteLine(
            $"Tax: ${CalculateTax():F2}");

        Console.WriteLine(
            $"Extra Fee: ${CalculateExtraFee():F2}");

        Console.WriteLine(
            $"Total: ${CalculateTotal():F2}");
    }
}