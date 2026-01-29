using System.Net;
using System.Net.Http.Json;
using CSharpApp001.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WeatherService.Tests;

public class GetCityDataTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GetCityDataTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCityData_ValidCountryAndCity_ReturnsOkStatus()
    {
        // Act
        var response = await _client.GetAsync("/countries/Portugal/Lisbon");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCityData_InvalidCountry_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/countries/InvalidCountry/InvalidCity");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCityData_InvalidCity_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/countries/Portugal/InvalidCity");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCityData_Lisbon_ReturnsAll12Months()
    {
        // Act
        var result = await _client.GetFromJsonAsync<Dictionary<string, TemperatureDto>>("/countries/Portugal/Lisbon");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(12, result.Count);
        Assert.True(result.ContainsKey("January"));
        Assert.True(result.ContainsKey("December"));
    }

    [Fact]
    public async Task GetCityData_Lisbon_ContainsValidTemperatureData()
    {
        // Act
        var result = await _client.GetFromJsonAsync<Dictionary<string, TemperatureDto>>("/countries/Portugal/Lisbon");

        // Assert
        Assert.NotNull(result);
        var january = result["January"];
        Assert.Equal(57, january.High);
        Assert.Equal(46, january.Low);
    }
}
