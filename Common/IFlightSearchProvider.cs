namespace Common;

public interface IFlightSearchProvider
{
    Task<IReadOnlyCollection<FlightSearch>> SearchAsync(string from, string to);
}