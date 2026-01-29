using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WeatherService.Tests;

public class GetCountriesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GetCountriesTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCountries_ReturnsOkStatus()
    {
        // Act
        var response = await _client.GetAsync("/countries");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCountries_ReturnsListOfCountries()
    {
        // Act
        var countries = await _client.GetFromJsonAsync<string[]>("/countries");

        // Assert
        Assert.NotNull(countries);
        Assert.True(countries.Length > 0);
    }

    [Fact]
    public async Task GetCountries_ContainsExpectedCountries()
    {
        // Act
        var countries = await _client.GetFromJsonAsync<string[]>("/countries");

        // Assert
        Assert.NotNull(countries);
        Assert.Contains("England", countries);
        Assert.Contains("France", countries);
        Assert.Contains("Germany", countries);
        Assert.Contains("Portugal", countries);
        Assert.Contains("Spain", countries);
        Assert.Contains("Italy", countries);
        Assert.Contains("Peru", countries);
    }
}
