using System.Net;
using System.Net.Http.Json;
using CSharpApp001.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WeatherService.Tests;

public class GetMonthlyAverageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GetMonthlyAverageTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetMonthlyAverage_ValidParameters_ReturnsOkStatus()
    {
        // Act
        var response = await _client.GetAsync("/countries/England/London/January");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetMonthlyAverage_InvalidCountry_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/countries/InvalidCountry/London/January");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMonthlyAverage_InvalidCity_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/countries/England/InvalidCity/January");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMonthlyAverage_InvalidMonth_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/countries/England/London/InvalidMonth");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMonthlyAverage_LondonJanuary_ReturnsCorrectTemperature()
    {
        // Act
        var result = await _client.GetFromJsonAsync<TemperatureDto>("/countries/England/London/January");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(45, result.High);
        Assert.Equal(36, result.Low);
    }

    [Fact]
    public async Task GetMonthlyAverage_SevilleJuly_ReturnsCorrectTemperature()
    {
        // Act
        var result = await _client.GetFromJsonAsync<TemperatureDto>("/countries/Spain/Seville/July");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(98, result.High);
        Assert.Equal(69, result.Low);
    }

    [Theory]
    [InlineData("England", "London", "January", 45, 36)]
    [InlineData("France", "Paris", "July", 75, 58)]
    [InlineData("Germany", "Berlin", "December", 38, 31)]
    [InlineData("Peru", "Lima", "August", 74, 62)]
    public async Task GetMonthlyAverage_VariousCities_ReturnsExpectedTemperature(
        string country, string city, string month, int expectedHigh, int expectedLow)
    {
        // Act
        var result = await _client.GetFromJsonAsync<TemperatureDto>($"/countries/{country}/{city}/{month}");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedHigh, result.High);
        Assert.Equal(expectedLow, result.Low);
    }
}
