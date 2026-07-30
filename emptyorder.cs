namespace RestaurantOrderSystem;

public class EmptyOrderException : Exception
{
    public EmptyOrderException()
        : base("An empty order cannot be completed.")
    {
    }
}