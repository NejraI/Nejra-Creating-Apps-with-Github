using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Api.Tests;

public class EndpointNameTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EndpointNameTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetEndpoint_Returns200AndExpectedData()
    {
        var response = await _client.GetAsync("/ENDPOINT_PATH");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
    }

    [Fact]
    public async Task GetEndpoint_Returns404ForNonexistent()
    {
        var response = await _client.GetAsync("/ENDPOINT_PATH/nonexistent");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}