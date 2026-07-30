using RestaurantOrderSystem.Enums;
using RestaurantOrderSystem.Interfaces;

namespace RestaurantOrderSystem.Models;

public abstract class MenuItem : IMenuItem
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public decimal Price { get; private set; }

    public ItemCategory Category { get; private set; }

    protected MenuItem(
        int id,
        string name,
        decimal price,
        ItemCategory category)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Item ID must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Item name cannot be empty.");
        }

        if (price < 0)
        {
            throw new ArgumentException(
                "Price cannot be negative.");
        }

        Id = id;
        Name = name;
        Price = price;
        Category = category;
    }

    public abstract string GetDescription();
}

public class FoodItem : MenuItem
{
    public bool IsVegetarian { get; private set; }

    public FoodItem(
        int id,
        string name,
        decimal price,
        ItemCategory category,
        bool isVegetarian)
        : base(id, name, price, category)
    {
        IsVegetarian = isVegetarian;
    }

    public override string GetDescription()
    {
        string type = IsVegetarian
            ? "Vegetarian"
            : "Regular";

        return $"{Id}. {Name} ({type}) - ${Price:F2}";
    }
}

public class DrinkItem : MenuItem
{
    public string Size { get; private set; }

    public DrinkItem(
        int id,
        string name,
        decimal price,
        string size)
        : base(
            id,
            name,
            price,
            ItemCategory.Drink)
    {
        Size = size;
    }

    public override string GetDescription()
    {
        return $"{Id}. {Name} ({Size}) - ${Price:F2}";
    }
}