namespace RestaurantOrderSystem.Models;

public class Reservation
{
    public int ReservationId { get; private set; }

    public string CustomerName { get; private set; }

    public DateTime ReservationTime { get; private set; }

    public int PartySize { get; private set; }

    public Reservation(int reservationId,string customerName, DateTime reservationTime,int partySize)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new ArgumentException("Customer name cannot be empty.");
        }

        if (reservationTime <= DateTime.Now)
        {
            throw new ArgumentException("Reservation must be in the future.");
        }

        if (partySize <= 0)
        {
            throw new ArgumentException("Party size must be greater than zero.");
        }

        ReservationId = reservationId;
        CustomerName = customerName;
        ReservationTime = reservationTime;
        PartySize = partySize;
    }
}