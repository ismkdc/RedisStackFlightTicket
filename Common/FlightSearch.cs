using Redis.OM.Modeling;

namespace Common;

[Document(StorageType = StorageType.Json)]
public class FlightSearch
{
    [RedisIdField] public Ulid Id { get; set; } // Unique identifier for the flight search entry

    [Indexed] public string From { get; set; } // Departure airport code (e.g., LAX, JFK)

    [Indexed] public string To { get; set; } // Arrival airport code (e.g., LHR, CDG)

    [Indexed(Sortable = true)] public DateTime DepartureDate { get; set; } // Date of flight departure

    [Indexed(Sortable = true)] public DateTime ReturnDate { get; set; } // Return date for round-trip flights (optional)

    [Indexed] public string Airline { get; set; } // Airline providing the flight (optional)

    [Indexed(Sortable = true)] public decimal Price { get; set; } // Price of the flight

    [Indexed] public string CabinClass { get; set; } // Cabin class (Economy, Business, First)

    public bool IsDirect { get; set; } // Indicates whether the flight is non-stop

    public int AvailableSeats { get; set; } // Number of seats available on the flight

    [Indexed(Sortable = true)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Date and time the entry was created
}