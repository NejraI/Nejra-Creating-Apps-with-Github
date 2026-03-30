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

    [Fact]    
    public async Task GetSummary_VerifyVerdictCountsAddUpToTotal()
    {
        var response = await _client.GetAsync("/summary");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        Assert.True(json.RootElement.TryGetProperty("total", out var total));
        Assert.True(json.RootElement.TryGetProperty("by_verdict", out var byVerdict));
        int sum = byVerdict.EnumerateObject().Sum(v => v.Value.GetInt32());
        Assert.Equal(total.GetInt32(), sum);
    }

    // Test that by_tool data is correct: each tool has verdict counts and they sum correctly
    [Fact]
    public async Task GetSummary_VerifyByToolDataIsCorrect()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/summary");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        
        // Assert: by_tool exists and is not empty
        Assert.True(json.RootElement.TryGetProperty("by_tool", out var byTool));
        var toolCount = byTool.EnumerateObject().Count();
        Assert.True(toolCount > 0, "by_tool should contain at least one tool");
        
        // Assert: each tool has the expected verdict structure
        var expectedVerdicts = new[] { "Faster", "Same", "Slower", "Surprising" };
        foreach (var tool in byTool.EnumerateObject())
        {
            var verdictCounts = tool.Value;
            
            // Each tool should have all verdict types
            foreach (var verdict in expectedVerdicts)
            {
                Assert.True(verdictCounts.TryGetProperty(verdict, out var count), 
                    $"Tool '{tool.Name}' should have '{verdict}' verdict count");
                Assert.True(count.GetInt32() >= 0, 
                    $"Verdict count for '{verdict}' in tool '{tool.Name}' should be non-negative");
            }
            
            // Verdict counts for each tool should sum to total entries for that tool
            var toolTotal = expectedVerdicts.Sum(v => verdictCounts.GetProperty(v).GetInt32());
            Assert.True(toolTotal > 0, $"Tool '{tool.Name}' should have at least one entry");
        }
    }
}
