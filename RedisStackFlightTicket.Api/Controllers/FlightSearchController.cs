using Common;
using Microsoft.AspNetCore.Mvc;
using Redis.OM;
using Redis.OM.Contracts;

namespace RedisStackFlightTicket.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FlightSearchController(
    IRedisConnectionProvider redisConnectionProvider,
    IEnumerable<IFlightSearchProvider> flightSearchProviders) : ControllerBase
{
    [HttpGet(Name = "FlightSearch")]
    public async IAsyncEnumerable<FlightSearch> Search(
        [FromQuery] string from,
        [FromQuery] string to
    )
    {
        var flightSearchCollection = redisConnectionProvider
            .RedisCollection<FlightSearch>();

        var breaker = false;
        await foreach (var flight in flightSearchCollection
                           .Where(x => x.From == from && x.To == to))
        {
            yield return flight;
            breaker = true;
        }

        if (breaker) yield break;

        var searchTasks = flightSearchProviders.Select(f => f.SearchAsync(from, to));
        await foreach (var searchResultsTask in Task.WhenEach(searchTasks))
        {
            var searchResults = await searchResultsTask;
            foreach (var searchResult in searchResults)
            {
                await flightSearchCollection.InsertAsync(searchResult, TimeSpan.FromDays(1));
                yield return searchResult;
            }
        }
    }
}