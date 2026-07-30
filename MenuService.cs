using RestaurantOrderSystem.Enums;
using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public class MenuService
{
    private readonly List<MenuItem> _menuItems = new()
    {
        new FoodItem(
            1,
            "Cheeseburger",
            12.50m,
            ItemCategory.MainDish,
            false),

        new FoodItem(
            2,
            "Vegetable Pasta",
            11.00m,
            ItemCategory.MainDish,
            true),

        new FoodItem(
            3,
            "French Fries",
            4.50m,
            ItemCategory.SideDish,
            true),

        new FoodItem(
            4,
            "Chocolate Cake",
            6.00m,
            ItemCategory.Dessert,
            true),

        new DrinkItem(
            5,
            "Cola",
            3.00m,
            "Medium"),

        new DrinkItem(
            6,
            "Coffee",
            3.50m,
            "Large")
    };

    public void DisplayMenu()
    {
        Console.WriteLine();
        Console.WriteLine("========== MENU ==========");

        foreach (MenuItem item in _menuItems)
        {
            Console.WriteLine(
                item.GetDescription());
        }
    }

    public MenuItem GetItem(int id)
    {
        MenuItem? item = _menuItems.FirstOrDefault(
            menuItem => menuItem.Id == id);

        if (item == null)
        {
            throw new ArgumentException(
                "Menu item was not found.");
        }

        return item;
    }

    public List<MenuItem> SearchByPrice(
        decimal maximumPrice)
    {
        return _menuItems
            .Where(item => item.Price <= maximumPrice)
            .OrderBy(item => item.Price)
            .ToList();
    }

    public List<FoodItem> GetVegetarianItems()
    {
        return _menuItems
            .OfType<FoodItem>()
            .Where(item => item.IsVegetarian)
            .ToList();
    }
}