using Bogus;
using Common;

namespace FlightSearchProvider.Moon;

public class MoonFlightSearchProvider : IFlightSearchProvider
{
    public async Task<IReadOnlyCollection<FlightSearch>> SearchAsync(string from, string to)
    {
        // Simulate some delay to mimic an async call
        await Task.Delay(Random.Shared.Next(3000, 5000));

        // Generate fake flight data using Bogus
        var flightFaker = new Faker<FlightSearch>()
            .RuleFor(f => f.Id, f => Ulid.NewUlid())
            .RuleFor(f => f.From, from) // Use the provided origin
            .RuleFor(f => f.To, to) // Use the provided destination
            .RuleFor(f => f.DepartureDate, f => f.Date.Future())
            .RuleFor(f => f.ReturnDate, f => f.Date.Future()) // Return date slightly in the future
            .RuleFor(f => f.Airline, f => f.Company.CompanyName())
            .RuleFor(f => f.Price, f => f.Finance.Amount(50)) // Random price between 50 and 1000
            .RuleFor(f => f.CabinClass, f => f.PickRandom("Economy", "Business", "First"))
            .RuleFor(f => f.IsDirect, f => f.Random.Bool())
            .RuleFor(f => f.AvailableSeats, f => f.Random.Int(1, 100));

        // Generate 5-10 fake flights
        var flights = flightFaker.Generate(Random.Shared.Next(1, 2));

        return flights.AsReadOnly();
    }
}