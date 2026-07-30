using Xunit;
using RestaurantOrderSystem;
using RestaurantOrderSystem.Enums;
using RestaurantOrderSystem.Models;
public class OrderTests
{
    [Fact]
    public void AddItem_AddsItemToOrder()
    {
        Order order = new(
            1,
            "Ayaka",
            OrderType.Takeout);

        FoodItem burger = new(
            1,
            "Burger",
            10.00m,
            ItemCategory.MainDish,
            false);

        order.AddItem(burger, 2);

        Assert.Single(order.Items);
        Assert.Equal(20.00m, order.CalculateSubtotal());
    }

    [Fact]
    public void AddSameItem_IncreasesQuantity()
    {
        Order order = new(
            2,
            "Ayaka",
            OrderType.Takeout);

        FoodItem fries = new(
            2,
            "Fries",
            4.00m,
            ItemCategory.SideDish,
            true);

        order.AddItem(fries, 1);
        order.AddItem(fries, 2);

        Assert.Single(order.Items);
        Assert.Equal(3, order.Items[0].Quantity);
    }

    [Fact]
    public void DeliveryOrder_AddsDeliveryFee()
    {
        Order order = new(
            3,
            "Ayaka",
            OrderType.Delivery);

        FoodItem pasta = new(
            3,
            "Pasta",
            10.00m,
            ItemCategory.MainDish,
            true);

        order.AddItem(pasta, 1);

        Assert.Equal(15.80m, order.CalculateTotal());
    }

    [Fact]
    public void CompleteEmptyOrder_ThrowsException()
    {
        Order order = new(
            4,
            "Ayaka",
            OrderType.DineIn);

        Assert.Throws<EmptyOrderException>(
            order.CompleteOrder);
    }

    [Fact]
    public void ZeroQuantity_ThrowsException()
    {
        Order order = new(
            5,
            "Ayaka",
            OrderType.Takeout);

        DrinkItem cola = new(
            5,
            "Cola",
            3.00m,
            "Medium");

        Assert.Throws<ArgumentException>(
            () => order.AddItem(cola, 0));
    }
}