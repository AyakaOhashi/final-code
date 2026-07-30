using System.Text.Json;
using RestaurantOrderSystem.Enums;
using RestaurantOrderSystem.Models;
using RestaurantOrderSystem.Services;

namespace RestaurantOrderSystem;

internal class Program
{
    private static async Task Main()
    {
        MenuService menuService = new();
        FileService fileService = new();
        ReservationService reservationService = new();

        Console.WriteLine(
            "Restaurant Order Management System");

        Order currentOrder = CreateOrder();

        bool running = true;

        while (running)
        {
            DisplayOptions();

            string choice =
                Console.ReadLine()?.Trim()
                ?? string.Empty;

            try
            {
                switch (choice)
                {
                    case "1":
                        menuService.DisplayMenu();
                        break;

                    case "2":
                        AddItem(
                            menuService,
                            currentOrder);
                        break;

                    case "3":
                        currentOrder.DisplayOrder();
                        break;

                    case "4":
                        await fileService
                            .SaveOrderAsync(currentOrder);

                        Console.WriteLine(
                            "Order saved.");
                        break;

                    case "5":
                        Order? loadedOrder =
                            await fileService
                                .LoadOrderAsync();

                        if (loadedOrder == null)
                        {
                            Console.WriteLine(
                                "No saved order found.");
                        }
                        else
                        {
                            currentOrder = loadedOrder;

                            Console.WriteLine(
                                "Order loaded.");
                        }

                        break;

                    case "6":
                        currentOrder.CompleteOrder();

                        Console.WriteLine(
                            "Processing payment...");

                        await Task.Delay(1000);

                        await fileService
                            .AddSaleAsync(currentOrder);

                        Console.WriteLine(
                            "Order completed.");

                        currentOrder = CreateOrder();
                        break;

                    case "7":
                        CreateReservation(
                            reservationService);
                        break;

                    case "8":
                        reservationService
                            .DisplayReservations();
                        break;

                    case "9":
                        SearchMenu(menuService);
                        break;

                    case "10":
                        List<Order> sales =
                            await fileService
                                .LoadSalesAsync();

                        SalesReportService
                            .DisplayReport(sales);
                        break;

                    case "0":
                        running = false;

                        Console.WriteLine(
                            "Program closed.");
                        break;

                    default:
                        Console.WriteLine(
                            "Please enter a valid option.");
                        break;
                }
            }
            catch (EmptyOrderException exception)
            {
                Console.WriteLine(
                    $"Order Error: {exception.Message}");
            }
            catch (JsonException)
            {
                Console.WriteLine(
                    "The JSON file is invalid.");
            }
            catch (IOException exception)
            {
                Console.WriteLine(
                    $"File Error: {exception.Message}");
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(
                    $"Input Error: {exception.Message}");
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"Unexpected Error: " +
                    $"{exception.Message}");
            }
        }
    }

    private static void DisplayOptions()
    {
        Console.WriteLine();
        Console.WriteLine("1. View Menu");
        Console.WriteLine("2. Add Item");
        Console.WriteLine("3. View Order");
        Console.WriteLine("4. Save Order");
        Console.WriteLine("5. Load Order");
        Console.WriteLine("6. Complete Order");
        Console.WriteLine("7. Create Reservation");
        Console.WriteLine("8. View Reservations");
        Console.WriteLine("9. Search Menu by Price");
        Console.WriteLine("10. Sales Report");
        Console.WriteLine("0. Exit");
        Console.Write("Choice: ");
    }

    private static Order CreateOrder()
    {
        Console.Write(
            "Customer name: ");

        string customerName =
            Console.ReadLine()?.Trim()
            ?? string.Empty;

        while (string.IsNullOrWhiteSpace(customerName))
        {
            Console.Write(
                "Please enter a customer name: ");

            customerName =
                Console.ReadLine()?.Trim()
                ?? string.Empty;
        }

        Console.WriteLine("1. Dine In");
        Console.WriteLine("2. Takeout");
        Console.WriteLine("3. Delivery");

        int orderTypeNumber =
            ReadNumber("Order type: ");

        OrderType orderType =
            orderTypeNumber switch
            {
                2 => OrderType.Takeout,
                3 => OrderType.Delivery,
                _ => OrderType.DineIn
            };

        return new Order(
            Random.Shared.Next(1000, 9999),
            customerName,
            orderType);
    }

    private static void AddItem(
        MenuService menuService,
        Order order)
    {
        menuService.DisplayMenu();

        int itemId =
            ReadNumber("Item ID: ");

        int quantity =
            ReadNumber("Quantity: ");

        MenuItem item =
            menuService.GetItem(itemId);

        order.AddItem(item, quantity);

        Console.WriteLine(
            $"{item.Name} was added.");
    }

    private static void CreateReservation(
        ReservationService reservationService)
    {
        Console.Write(
            "Customer name: ");

        string customerName =
            Console.ReadLine()?.Trim()
            ?? string.Empty;

        Console.Write(
            "Date and time " +
            "(example: 2026-07-30 18:00): ");

        string input =
            Console.ReadLine()?.Trim()
            ?? string.Empty;

        if (!DateTime.TryParse(
                input,
                out DateTime reservationTime))
        {
            Console.WriteLine(
                "Invalid date and time.");

            return;
        }

        int partySize =
            ReadNumber("Party size: ");

        Reservation reservation =
            reservationService.CreateReservation(
                customerName,
                reservationTime,
                partySize);

        Console.WriteLine(
            $"Reservation " +
            $"#{reservation.ReservationId} created.");
    }

    private static void SearchMenu(
        MenuService menuService)
    {
        Console.Write(
            "Maximum price: $");

        string input =
            Console.ReadLine()?.Trim()
            ?? string.Empty;

        if (!decimal.TryParse(
                input,
                out decimal maximumPrice))
        {
            Console.WriteLine(
                "Invalid price.");

            return;
        }

        List<MenuItem> results =
            menuService.SearchByPrice(
                maximumPrice);

        foreach (MenuItem item in results)
        {
            Console.WriteLine(
                item.GetDescription());
        }
    }

    private static int ReadNumber(
        string message)
    {
        while (true)
        {
            Console.Write(message);

            string input =
                Console.ReadLine()?.Trim()
                ?? string.Empty;

            if (int.TryParse(input, out int number)
                && number > 0)
            {
                return number;
            }

            Console.WriteLine(
                "Please enter a positive number.");
        }
    }
}