using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
app.UseCors();

var dataFile = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "data", "experiments.json");

List<Dictionary<string, object>> ReadExperiments()
{
    var json = File.ReadAllText(dataFile);
    return JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json)!;
}

void WriteExperiments(List<Dictionary<string, object>> data)
{
    var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(dataFile, json);
}

// Serve the frontend as static files
var frontendProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
    Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "frontend"));

app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = frontendProvider, RequestPath = "" });
app.UseStaticFiles(new StaticFileOptions { FileProvider = frontendProvider, RequestPath = "" });

// GET /entries — return all log entries
app.MapGet("/entries", () =>
{
    var experiments = ReadExperiments();
    return Results.Ok(experiments);
});

// GET /entries/{id} — return one entry by id
app.MapGet("/entries/{id}", (string id) =>
{
    var experiments = ReadExperiments();
    var entry = experiments.FirstOrDefault(e =>
        e.TryGetValue("id", out var val) && val.ToString() == id);
    return entry is null
        ? Results.NotFound(new { error = "Entry not found" })
        : Results.Ok(entry);
});

// POST /entries — add a new entry
app.MapPost("/entries", async (HttpRequest request) =>
{
    var body = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(request.Body);
    if (body is null) return Results.BadRequest(new { error = "Invalid request body" });

    string[] required = ["tool", "task", "expected", "actual", "verdict"];
    if (!required.All(f => body.ContainsKey(f) && !string.IsNullOrWhiteSpace(body[f])))
    {
        return Results.BadRequest(new { error = "All fields are required: tool, task, expected, actual, verdict" });
    }

    var experiments = ReadExperiments();
    var newEntry = new Dictionary<string, object>
    {
        ["id"] = $"exp-{Guid.NewGuid().ToString("N")[..8]}",
        ["tool"] = body["tool"],
        ["task"] = body["task"],
        ["expected"] = body["expected"],
        ["actual"] = body["actual"],
        ["verdict"] = body["verdict"],
        ["timestamp"] = DateTime.UtcNow.ToString("o")
    };
    experiments.Add(newEntry);
    WriteExperiments(experiments);
    return Results.Created($"/entries/{newEntry["id"]}", newEntry);
});

// GET /summary — aggregated experiment data
// Returns a JSON object with this shape:
// {
//   "total": <number of entries>,
//   "by_verdict": { "Faster": <count>, "Same": <count>, ... },
//   "by_tool": { "<tool name>": { "Faster": <count>, ... } }
// }
app.MapGet("/summary", () =>
{
    // Read all experiments and count verdicts
    var experiments = ReadExperiments();

    var verdicts = new[] { "Faster", "Same", "Slower", "Surprising" };
    var byVerdict = new Dictionary<string, int>();
    foreach (var v in verdicts) byVerdict[v] = 0;

    foreach (var exp in experiments)
    {
        var verdict = exp.TryGetValue("verdict", out var v2) ? v2.ToString() ?? "" : "";
        if (byVerdict.ContainsKey(verdict))
        {
            byVerdict[verdict]++;
        }
    }

    // Group verdict counts per tool
    var byTool = new Dictionary<string, Dictionary<string, int>>();
    foreach (var exp in experiments)
    {
        var tool = exp.TryGetValue("tool", out var t) ? t.ToString() ?? "" : "";
        if (!byTool.ContainsKey(tool))
        {
            byTool[tool] = new Dictionary<string, int>();
            foreach (var v in verdicts) byTool[tool][v] = 0;
        }
        var verdict = exp.TryGetValue("verdict", out var v3) ? v3.ToString() ?? "" : "";
        if (byTool[tool].ContainsKey(verdict))
        {
            byTool[tool][verdict]++;
        }
    }

    return Results.Ok(new
    {
        total = experiments.Count,
        by_verdict = byVerdict,
        by_tool = byTool,
    });
});

app.Run("http://localhost:5001");
