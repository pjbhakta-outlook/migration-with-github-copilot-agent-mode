using System.Text.Json;
using CSharpApp001.Models;

namespace CSharpApp001.Services;

/// <summary>
/// Service for accessing weather data from JSON file.
/// </summary>
public class WeatherService : IWeatherService
{
    private readonly Dictionary<string, Dictionary<string, Dictionary<string, TemperatureDto>>> _data;

    public WeatherService()
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "weather.json");
        var jsonContent = File.ReadAllText(jsonPath);
        
        _data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, TemperatureDto>>>>(
            jsonContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        ) ?? new();
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetCountries() => _data.Keys;

    /// <inheritdoc/>
    public TemperatureDto? GetMonthlyAverage(string country, string city, string month)
    {
        if (_data.TryGetValue(country, out var cities) &&
            cities.TryGetValue(city, out var months) &&
            months.TryGetValue(month, out var temperature))
        {
            return temperature;
        }
        return null;
    }

    /// <inheritdoc/>
    public IEnumerable<string>? GetCities(string country)
    {
        return _data.TryGetValue(country, out var cities) ? cities.Keys : null;
    }

    /// <inheritdoc/>
    public Dictionary<string, TemperatureDto>? GetCityData(string country, string city)
    {
        if (_data.TryGetValue(country, out var cities) &&
            cities.TryGetValue(city, out var months))
        {
            return months;
        }
        return null;
    }
}
