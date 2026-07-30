using System.Text.Json;
using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public class FileService
{
    private const string OrderFile = "order.json";
    private const string SalesFile = "sales.json";

    private readonly SemaphoreSlim _fileLock =
        new(1, 1);

    private readonly JsonSerializerOptions _options =
        new()
        {
            WriteIndented = true
        };

    public async Task SaveOrderAsync(Order order)
    {
        await _fileLock.WaitAsync();

        try
        {
            string json = JsonSerializer.Serialize(
                order,
                _options);

            await File.WriteAllTextAsync(
                OrderFile,
                json);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<Order?> LoadOrderAsync()
    {
        await _fileLock.WaitAsync();

        try
        {
            if (!File.Exists(OrderFile))
            {
                return null;
            }

            string json =
                await File.ReadAllTextAsync(OrderFile);

            return JsonSerializer.Deserialize<Order>(
                json,
                _options);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task AddSaleAsync(Order order)
    {
        await _fileLock.WaitAsync();

        try
        {
            List<Order> sales =
                await ReadSalesWithoutLockAsync();

            sales.Add(order);

            string json = JsonSerializer.Serialize(
                sales,
                _options);

            await File.WriteAllTextAsync(
                SalesFile,
                json);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<List<Order>> LoadSalesAsync()
    {
        await _fileLock.WaitAsync();

        try
        {
            return await ReadSalesWithoutLockAsync();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task<List<Order>>
        ReadSalesWithoutLockAsync()
    {
        if (!File.Exists(SalesFile))
        {
            return new List<Order>();
        }

        string json =
            await File.ReadAllTextAsync(SalesFile);

        return JsonSerializer
                   .Deserialize<List<Order>>(
                       json,
                       _options)
               ?? new List<Order>();
    }
}