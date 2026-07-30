namespace RestaurantOrderSystem.Interfaces;

public interface IMenuItem
{
    int Id { get; }

    string Name { get; }

    decimal Price { get; }

    string GetDescription();
}