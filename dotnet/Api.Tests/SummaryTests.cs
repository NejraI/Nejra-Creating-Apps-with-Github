using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests;

public class SummaryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SummaryTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetSummary_Returns200AndExpectedShape()
    {
        var response = await _client.GetAsync("/summary");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        Assert.True(json.RootElement.TryGetProperty("total", out _));
        Assert.True(json.RootElement.TryGetProperty("by_verdict", out _));
    }

    // Lab 2 Challenge: add more tests below this line

    // Test that by_tool contains the correct tool names from the data
    [Fact]
    public async Task GetSummary_ReturnsByToolWithToolNames()
    {
        var response = await _client.GetAsync("/summary");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        Assert.True(json.RootElement.TryGetProperty("by_tool", out var byTool));
        Assert.True(byTool.EnumerateObject().Any());
    }

    // Test that the verdict counts in by_verdict add up to the total
    [Fact]
    public async Task GetSummary_VerdictCountsAddUpToTotal()
    {
        var response = await _client.GetAsync("/summary");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        var total = json.RootElement.GetProperty("total").GetInt32();
        var byVerdict = json.RootElement.GetProperty("by_verdict");
        var verdictSum = 0;
        foreach (var prop in byVerdict.EnumerateObject())
        {
            verdictSum += prop.Value.GetInt32();
        }
        Assert.Equal(total, verdictSum);
    }
}
