using Scalar.AspNetCore;
using CSharpApp001.Models;
using CSharpApp001.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IWeatherService, WeatherService>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

// Root endpoint - redirects to Scalar API documentation
app.MapGet("/", () => Results.Redirect("/scalar/v1"))
    .WithName("Root")
    .WithSummary("API Documentation")
    .WithDescription("Redirects to the Scalar API documentation page.")
    .ExcludeFromDescription();

// Get all countries
app.MapGet("/countries", (IWeatherService weatherService) => weatherService.GetCountries())
    .WithName("GetCountries")
    .WithSummary("Get all countries")
    .WithDescription("Returns a list of all available countries with weather data.")
    .WithTags("Weather")
    .Produces<IEnumerable<string>>(StatusCodes.Status200OK);

// Get all cities in a country
app.MapGet("/countries/{country}/cities", (string country, IWeatherService weatherService) =>
{
    var result = weatherService.GetCities(country);
    return result is not null ? Results.Ok(result) : Results.NotFound();
})
    .WithName("GetCities")
    .WithSummary("Get cities in a country")
    .WithDescription("Returns a list of all cities with weather data for the specified country.")
    .WithTags("Weather")
    .Produces<IEnumerable<string>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

// Get all monthly temperature data for a city
app.MapGet("/countries/{country}/{city}", (string country, string city, IWeatherService weatherService) =>
{
    var result = weatherService.GetCityData(country, city);
    return result is not null ? Results.Ok(result) : Results.NotFound();
})
    .WithName("GetCityData")
    .WithSummary("Get all temperature data for a city")
    .WithDescription("Returns temperature data for all 12 months for the specified city.")
    .WithTags("Weather")
    .Produces<Dictionary<string, TemperatureDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

// Get monthly average temperature for a specific country, city, and month
app.MapGet("/countries/{country}/{city}/{month}", (string country, string city, string month, IWeatherService weatherService) =>
{
    var result = weatherService.GetMonthlyAverage(country, city, month);
    return result is not null ? Results.Ok(result) : Results.NotFound();
})
    .WithName("GetMonthlyAverage")
    .WithSummary("Get monthly average temperature")
    .WithDescription("Returns the high and low temperature for a specific country, city, and month.")
    .WithTags("Weather")
    .Produces<TemperatureDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.Run();

// Needed for WebApplicationFactory in integration tests
public partial class Program { }
