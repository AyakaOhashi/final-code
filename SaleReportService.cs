using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public static class SalesReportService
{
    public static void DisplayReport(
        List<Order> orders)
    {
        Console.WriteLine();
        Console.WriteLine("===== SALES REPORT =====");

        Console.WriteLine(
            $"Completed Orders: {orders.Count}");

        decimal totalRevenue = orders.Sum(
            order => order.CalculateTotal());

        Console.WriteLine(
            $"Total Revenue: ${totalRevenue:F2}");

        var topItems = orders
            .SelectMany(order => order.Items)
            .GroupBy(item => item.Name)
            .Select(group => new
            {
                Name = group.Key,

                Quantity = group.Sum(
                    item => item.Quantity)
            })
            .OrderByDescending(
                result => result.Quantity)
            .Take(3);

        Console.WriteLine("Popular Items:");

        foreach (var item in topItems)
        {
            Console.WriteLine(
                $"{item.Name}: {item.Quantity}");
        }
    }
}