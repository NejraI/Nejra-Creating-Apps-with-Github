# Lab 2 — Core Guided: Add Tool Grouping to the Summary Endpoint

**Goal:** Use Copilot inline completion to add `by_tool` grouping to the existing `/summary` endpoint, unlocking the Tool Comparison chart.

**Time:** 15 minutes

**You will need:** Lab 1 completed, Copilot instructions file in place, API running in your Codespace.

> Note: If you are not finished in time, copy the completed file from the `rescue/` folder for your language into your project folder (see the [rescue README](../../rescue/README.md) for details), commit, and push. You will continue with Labs 3 and 4 from there.

---

## Background

Your API already has a working `/summary` endpoint that returns `total` and `by_verdict`. If you open the Dashboard, you can see the **Verdict Breakdown** doughnut chart is already working. But the **Tool Comparison** chart shows a placeholder: *"Add `by_tool` to unlock this chart."*

In this lab you will:

1. Look at how `by_verdict` is implemented (it is your pattern to follow)
2. Use Copilot inline completion to add the `by_tool` grouping
3. Verify the Tool Comparison chart appears
4. Commit your changes

The `by_tool` field should be a nested object where each key is a tool name, and the value is a verdict breakdown for that tool.

---

## Steps

### Part A: See What Already Works

1. Start your API server if it is not already running:

   - **Node:**
     ```bash
     cd node
     npm start
     ```

   - **Python:**
     ```bash
     cd python
     pip install -r requirements.txt
     python app.py
     ```

   - **.NET:**
     ```bash
     cd dotnet
     dotnet run
     ```

2. Open the app in your browser. In a Codespace, click the **Ports** tab and open the forwarded port (3000 for Node, 5000 for Python, 5001 for .NET).

3. Click the **Dashboard** tab. You should see:
   - A **Verdict Breakdown** doughnut chart (already working)
   - A **Tool Comparison** area with the message: *"Add `by_tool` to unlock this chart."*
   - A total experiment count at the top

4. Test the current endpoint in a terminal to see what it returns:

   ```bash
   curl http://localhost:3000/summary   # Node
   curl http://localhost:5000/summary   # Python
   curl http://localhost:5001/summary   # .NET
   ```

   You should see JSON with `total` and `by_verdict`, but **no `by_tool`** yet.

5. Stop the API server (press `Ctrl+C`) so you can edit the file.

### Part B: Find the TODO

6. Open your API file:
   - **Node:** `node/server.js`
   - **Python:** `python/app.py`
   - **.NET:** `dotnet/Program.cs`

7. Find the `/summary` endpoint. Look for the TODO comment:

   **Node:**
   ```javascript
   // TODO: Add by_tool grouping — Lab 2
   // Add a byTool object that groups verdict counts per tool.
   // The frontend expects: { "<tool name>": { "Faster": 0, "Same": 0, ... } }
   ```

   **Python:**
   ```python
   # TODO: Add by_tool grouping — Lab 2
   # Add a by_tool dict that groups verdict counts per tool.
   # The frontend expects: { "<tool name>": { "Faster": 0, "Same": 0, ... } }
   ```

   **C#:**
   ```csharp
   // TODO: Add by_tool grouping — Lab 2
   // Add a byTool dictionary that groups verdict counts per tool.
   // The frontend expects: { "<tool name>": { "Faster": 0, "Same": 0, ... } }
   ```

8. Read the existing `by_verdict` code above the TODO. Notice the pattern:
   - A dictionary is initialized with zero counts
   - A loop increments the count for each experiment's verdict
   - The result is included in the JSON response

   The `by_tool` grouping follows the same idea, but nests a verdict breakdown inside each tool name.

### Part C: Use Copilot to Add by_tool

9. Place your cursor at the end of the TODO comment block (after the last `//` line). Delete the TODO comment lines.

10. Write a descriptive comment that tells Copilot what you want. For example:

    - **Node:**
      ```javascript
      // Group experiments by tool name, with verdict counts for each tool
      ```

    - **Python:**
      ```python
      # Group experiments by tool name, with verdict counts for each tool
      ```

    - **.NET:**
      ```csharp
      // Group experiments by tool name, with verdict counts for each tool
      ```

11. Press **Enter** at the end of the comment line. Copilot should show a grey inline suggestion. Wait a moment for it to appear.

    > **What to look for:** Copilot should suggest code that:
    > 1. Creates an empty `by_tool` / `byTool` dictionary
    > 2. Loops through experiments (or extends the existing loop)
    > 3. For each experiment, gets the tool name
    > 4. If the tool hasn't been seen before, initializes it with zero verdict counts
    > 5. Increments the count for that tool's verdict

12. Press **Tab** to accept the suggestion. If it only gives part of the code, press **Enter** and accept the next suggestion.

13. **Important:** Make sure `by_tool` is included in the return value. Check the `return` / `res.json()` / `Results.Ok()` at the bottom of the function and add `by_tool` to the JSON response if Copilot didn't do it automatically.

    - **Node:** `res.json({ total: experiments.length, by_verdict: byVerdict, by_tool: byTool });`
    - **Python:** `return jsonify({ "total": len(experiments), "by_verdict": by_verdict, "by_tool": by_tool })`
    - **.NET:** `return Results.Ok(new { total = experiments.Count, by_verdict = byVerdict, by_tool = byTool });`

14. If Copilot's suggestion doesn't look right:
    - **Be more specific:** e.g., `// Create a byTool dictionary, loop through experiments, initialize new tools with { Faster: 0, Same: 0, Slower: 0, Surprising: 0 }, and increment the matching verdict`
    - **Start writing code yourself:** e.g., type `const byTool = {};` and let Copilot continue
    - **Use Copilot Chat:** Ask `Add by_tool grouping to the /summary endpoint based on the existing by_verdict pattern`

### Part D: Test It

15. Save the file and restart the API server.

16. Test the endpoint in a terminal:

    ```bash
    curl http://localhost:3000/summary   # Node
    curl http://localhost:5000/summary   # Python
    curl http://localhost:5001/summary   # .NET
    ```

    You should now see JSON with all three keys: `total`, `by_verdict`, **and** `by_tool`. For example:

    ```json
    {
      "total": 6,
      "by_verdict": {
        "Faster": 3,
        "Same": 1,
        "Slower": 1,
        "Surprising": 1
      },
      "by_tool": {
        "Copilot Inline": {
          "Faster": 2,
          "Same": 0,
          "Slower": 0,
          "Surprising": 0
        },
        "Copilot Chat": {
          "Faster": 0,
          "Same": 1,
          "Slower": 0,
          "Surprising": 1
        },
        "Codespaces": {
          "Faster": 1,
          "Same": 0,
          "Slower": 1,
          "Surprising": 0
        }
      }
    }
    ```

17. Open the Dashboard in your browser. You should now see:
    - The **Tool Comparison** chart with stacked bars for each tool
    - The placeholder message is **gone**

### Part E: Commit Your Changes

18. Stop the API server (`Ctrl+C`).

19. Stage and commit your changes:

    ```bash
    git add -A
    git commit -m "Add by_tool grouping to /summary endpoint, closes #1"
    ```

20. Push your changes:

    ```bash
    git push
    ```

---

## What You Should Have by the End

### Node — `node/server.js` (summary endpoint only)

The `/summary` route handler should now include `by_tool` grouping alongside the existing `by_verdict`:

```javascript
app.get("/summary", (req, res) => {
  const experiments = readExperiments();

  const byVerdict = { Faster: 0, Same: 0, Slower: 0, Surprising: 0 };
  const byTool = {};

  for (const exp of experiments) {
    if (exp.verdict in byVerdict) {
      byVerdict[exp.verdict]++;
    }

    if (!byTool[exp.tool]) {
      byTool[exp.tool] = { Faster: 0, Same: 0, Slower: 0, Surprising: 0 };
    }
    if (exp.verdict in byTool[exp.tool]) {
      byTool[exp.tool][exp.verdict]++;
    }
  }

  res.json({
    total: experiments.length,
    by_verdict: byVerdict,
    by_tool: byTool,
  });
});
```

### Python — `python/app.py` (summary endpoint only)

```python
@app.route("/summary", methods=["GET"])
def get_summary():
    """Aggregated experiment data."""
    experiments = read_experiments()

    by_verdict = {"Faster": 0, "Same": 0, "Slower": 0, "Surprising": 0}
    by_tool = {}

    for exp in experiments:
        verdict = exp.get("verdict", "")
        tool = exp.get("tool", "")

        if verdict in by_verdict:
            by_verdict[verdict] += 1

        if tool not in by_tool:
            by_tool[tool] = {"Faster": 0, "Same": 0, "Slower": 0, "Surprising": 0}
        if verdict in by_tool[tool]:
            by_tool[tool][verdict] += 1

    return jsonify({
        "total": len(experiments),
        "by_verdict": by_verdict,
        "by_tool": by_tool,
    })
```

### .NET — `dotnet/Program.cs` (summary endpoint only)

```csharp
app.MapGet("/summary", () =>
{
    var experiments = ReadExperiments();

    var verdicts = new[] { "Faster", "Same", "Slower", "Surprising" };
    var byVerdict = new Dictionary<string, int>();
    foreach (var v in verdicts) byVerdict[v] = 0;

    var byTool = new Dictionary<string, Dictionary<string, int>>();

    foreach (var exp in experiments)
    {
        var verdict = exp.TryGetValue("verdict", out var v2) ? v2.ToString() ?? "" : "";
        var tool = exp.TryGetValue("tool", out var t) ? t.ToString() ?? "" : "";

        if (byVerdict.ContainsKey(verdict))
        {
            byVerdict[verdict]++;
        }

        if (!byTool.ContainsKey(tool))
        {
            byTool[tool] = new Dictionary<string, int>();
            foreach (var vv in verdicts) byTool[tool][vv] = 0;
        }
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
```

### What the Dashboard Should Look Like

When the endpoint is working correctly:
- The **verdict doughnut chart** shows colored slices — green for Faster, blue for Same, red for Slower, amber for Surprising
- The **tool comparison bar chart** shows a stacked bar for each tool, with the same color coding
- The **total count** at the top matches the number of entries in the Log view

### What Should NOT Change

- The `readExperiments()` / `read_experiments()` / `ReadExperiments()` helper function — you reuse it, not rewrite it
- The other endpoints (`GET /entries`, `GET /entries/:id`, `POST /entries`) — leave them untouched
- The `frontend/` folder — the dashboard code already handles rendering the data

---

## Troubleshooting

| Problem | Likely Cause | Fix |
|---------|-------------|-----|
| Tool chart still shows placeholder | `by_tool` not in the JSON response | Make sure you added `by_tool` to the return statement |
| `curl` shows `by_tool` is empty `{}` | Loop doesn't read the tool field | Check that you're reading `exp.tool` / `exp["tool"]` / `exp.TryGetValue("tool", ...)` |
| All tool counts are zero | Verdict counting logic error in the tool loop | Verify you're incrementing `byTool[tool][verdict]` inside the loop |
| Server crashes on start | Syntax error in your code | Read the error message in the terminal — it will show the line number |

**Expected output:** The `/summary` endpoint returns 200 with `total`, `by_verdict`, and `by_tool`. Both dashboard charts are visible.

You're doing great — watching that second chart appear is one of the best moments in the workshop!
