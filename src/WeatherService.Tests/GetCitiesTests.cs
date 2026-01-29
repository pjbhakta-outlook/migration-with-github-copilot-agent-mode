using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WeatherService.Tests;

public class GetCitiesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GetCitiesTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCities_ValidCountry_ReturnsOkStatus()
    {
        // Act
        var response = await _client.GetAsync("/countries/Portugal/cities");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCities_InvalidCountry_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/countries/InvalidCountry/cities");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCities_Portugal_ReturnsLisbonAndPorto()
    {
        // Act
        var cities = await _client.GetFromJsonAsync<string[]>("/countries/Portugal/cities");

        // Assert
        Assert.NotNull(cities);
        Assert.Contains("Lisbon", cities);
        Assert.Contains("Porto", cities);
    }

    [Fact]
    public async Task GetCities_England_ReturnsLondon()
    {
        // Act
        var cities = await _client.GetFromJsonAsync<string[]>("/countries/England/cities");

        // Assert
        Assert.NotNull(cities);
        Assert.Contains("London", cities);
    }
}
