using CSharpApp001.Models;

namespace CSharpApp001.Services;

/// <summary>
/// Service interface for weather data operations.
/// </summary>
public interface IWeatherService
{
    /// <summary>
    /// Gets all available countries.
    /// </summary>
    /// <returns>Collection of country names.</returns>
    IEnumerable<string> GetCountries();

    /// <summary>
    /// Gets monthly average temperature for a specific country, city, and month.
    /// </summary>
    /// <param name="country">The country name.</param>
    /// <param name="city">The city name.</param>
    /// <param name="month">The month name.</param>
    /// <returns>Temperature data if found, null otherwise.</returns>
    TemperatureDto? GetMonthlyAverage(string country, string city, string month);

    /// <summary>
    /// Gets all cities in a country.
    /// </summary>
    /// <param name="country">The country name.</param>
    /// <returns>Collection of city names, or null if country not found.</returns>
    IEnumerable<string>? GetCities(string country);

    /// <summary>
    /// Gets all monthly temperature data for a city.
    /// </summary>
    /// <param name="country">The country name.</param>
    /// <param name="city">The city name.</param>
    /// <returns>Dictionary of month to temperature data, or null if not found.</returns>
    Dictionary<string, TemperatureDto>? GetCityData(string country, string city);
}
