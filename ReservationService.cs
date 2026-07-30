using RestaurantOrderSystem.Models;

namespace RestaurantOrderSystem.Services;

public class ReservationService
{
    private readonly Dictionary<int, Reservation>
        _reservations = new();

    private int _nextId = 1;

    public Reservation CreateReservation(
        string customerName,
        DateTime reservationTime,
        int partySize)
    {
        Reservation reservation = new(
            _nextId,
            customerName,
            reservationTime,
            partySize);

        _reservations.Add(
            reservation.ReservationId,
            reservation);

        _nextId++;

        return reservation;
    }

    public List<Reservation> GetReservations()
    {
        return _reservations.Values
            .OrderBy(
                reservation =>
                    reservation.ReservationTime)
            .ToList();
    }

    public void DisplayReservations()
    {
        List<Reservation> reservations =
            GetReservations();

        Console.WriteLine();
        Console.WriteLine("===== RESERVATIONS =====");

        if (reservations.Count == 0)
        {
            Console.WriteLine(
                "No reservations found.");

            return;
        }

        foreach (Reservation reservation
                 in reservations)
        {
            Console.WriteLine(
                $"#{reservation.ReservationId} | " +
                $"{reservation.CustomerName} | " +
                $"{reservation.ReservationTime:g} | " +
                $"Party of {reservation.PartySize}");
        }
    }
}