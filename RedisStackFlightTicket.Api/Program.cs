using Common;
using FlightSearchProvider.Mars;
using FlightSearchProvider.Moon;
using FlightSearchProvider.Venus;
using Redis.OM;
using Redis.OM.Contracts;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        b.WithOrigins("http://localhost:8000");
        b.WithMethods("GET", "POST");
        b.AllowAnyHeader();
    });
});

var provider = new RedisConnectionProvider(
    new ConfigurationOptions
    {
        EndPoints = new EndPointCollection
        {
            {"localhost", 6379}
        },
        Password = "mypassword"
    });

var type = typeof(FlightSearch);
if (!await provider.Connection.IsIndexCurrentAsync(type))
{
    await provider.Connection.DropIndexAsync(type);
    await provider.Connection.CreateIndexAsync(type);
}
else
{
    await provider.Connection.CreateIndexAsync(type);
}

builder.Services.AddSingleton<IRedisConnectionProvider>(provider);

builder.Services.AddSingleton<IFlightSearchProvider, MarsFlightSearchProvider>();
builder.Services.AddSingleton<IFlightSearchProvider, VenusFlightSearchProvider>();
builder.Services.AddSingleton<IFlightSearchProvider, MoonFlightSearchProvider>();

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();