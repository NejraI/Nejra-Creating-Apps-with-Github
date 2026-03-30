using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests;

public class EntriesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EntriesTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // Test that GET /entries returns 200 and a JSON array
    [Fact]
    public async Task GetEntries_Returns200AndJsonArray()
    {
        var response = await _client.GetAsync("/entries");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        Assert.Equal(JsonValueKind.Array, json.RootElement.ValueKind);
    }

    // Test that GET /entries returns entries with expected fields
    [Fact]
    public async Task GetEntries_ReturnsEntriesWithExpectedFields()
    {
        var response = await _client.GetAsync("/entries");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        
        if (json.RootElement.GetArrayLength() > 0)
        {
            var firstEntry = json.RootElement[0];
            Assert.True(firstEntry.TryGetProperty("id", out _));
            Assert.True(firstEntry.TryGetProperty("tool", out _));
            Assert.True(firstEntry.TryGetProperty("task", out _));
            Assert.True(firstEntry.TryGetProperty("timestamp", out _));
        }
    }

    // Test that GET /entries/{id} returns 404 for nonexistent entry
    [Fact]
    public async Task GetEntryById_Returns404ForNonexistentId()
    {
        var response = await _client.GetAsync("/entries/nonexistent-id");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // Test that GET /entries/{id} returns 200 and correct entry when it exists
    [Fact]
    public async Task GetEntryById_Returns200AndEntryWhenExists()
    {
        // First, get all entries to find a real ID
        var listResponse = await _client.GetAsync("/entries");
        listResponse.EnsureSuccessStatusCode();
        var listContent = await listResponse.Content.ReadAsStringAsync();
        var listJson = JsonDocument.Parse(listContent);
        
        if (listJson.RootElement.GetArrayLength() > 0)
        {
            var firstEntry = listJson.RootElement[0];
            var firstId = firstEntry.GetProperty("id").GetString();
            
            var response = await _client.GetAsync($"/entries/{firstId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);
            Assert.Equal(firstId, json.RootElement.GetProperty("id").GetString());
        }
    }
}
