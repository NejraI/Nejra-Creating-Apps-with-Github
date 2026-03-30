# Lab 1 — Challenge Guided: Skills with Resources & Community Plugins

**Goal:** Create a skill that includes supporting resources alongside its instructions, and install a community plugin from the Awesome Copilot marketplace.

**Time:** 25 minutes

**You will need:** Lab 1 Core completed (instructions file and security review skill exist).

---

## Part 1 — Create a Testing Skill with Resources

Skills can include more than just a `SKILL.md` — you can add scripts, templates, examples, and other files to the skill folder. Copilot loads these resources on demand when they are referenced from the instructions.

1. Create the folder `.github/skills/api-testing/`
2. Inside that folder, create a resource file with a test template for your language. Name it based on your choice:
   - **Node:** `test-template.js`
   - **Python:** `test_template.py`
   - **dotnet:** `TestTemplate.cs`

3. Add a basic test skeleton to your template file.

   **Node/Jest** (`test-template.js`):

   ```javascript
   const request = require('supertest');
   const app = require('../../node/server');

   describe('ENDPOINT_NAME', () => {
     it('should return 200 and the expected data', async () => {
       const response = await request(app).get('/ENDPOINT_PATH');
       expect(response.status).toBe(200);
       expect(response.body).toBeDefined();
     });

     it('should handle errors gracefully', async () => {
       const response = await request(app).get('/ENDPOINT_PATH/nonexistent');
       expect(response.status).toBe(404);
     });
   });
   ```

   **Python/pytest** (`test_template.py`):

   ```python
   import pytest
   from app import app


   @pytest.fixture
   def client():
       app.config['TESTING'] = True
       with app.test_client() as client:
           yield client


   def test_endpoint_returns_200_and_expected_data(client):
       response = client.get('/ENDPOINT_PATH')
       assert response.status_code == 200
       assert response.get_json() is not None


   def test_endpoint_handles_errors_gracefully(client):
       response = client.get('/ENDPOINT_PATH/nonexistent')
       assert response.status_code == 404
   ```

   **.NET/xUnit** (`TestTemplate.cs`):

   ```csharp
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
   ```

4. Now create `SKILL.md` in the same folder (`.github/skills/api-testing/SKILL.md`)
5. Add YAML frontmatter with a `name` that matches the folder and a clear `description`:

   ```markdown
   ---
   name: api-testing
   description: "Write tests for the AI Experiment Log API. Use this skill when writing, scaffolding, or reviewing tests for API endpoints."
   ---
   ```

6. Add the skill body below the frontmatter. Reference your template file using a relative path:

   ```markdown
   # API Testing

   When writing tests for the AI Experiment Log API:

   1. Use the test template as a starting point:
      - Node: [test-template.js](./test-template.js)
      - Python: [test_template.py](./test_template.py)
      - dotnet: [TestTemplate.cs](./TestTemplate.cs)
   2. Place test files in the appropriate folder for the language:
      - Node: `node/__tests__/`
      - Python: `python/tests/`
      - dotnet: `dotnet/Api.Tests/`
   3. Each test should: arrange test data, act by calling the endpoint, assert the expected result
   4. Test both the happy path and at least one error case (e.g., missing ID returning 404)
   5. The API reads from `/data/experiments.json` — tests can use this data or mock it
   ```

   > **Note:** Update the template file reference in step 1 to match the file you created in step 2 (e.g., `test_template.py` for Python, `TestTemplate.cs` for .NET).

7. Save both files. Your skill folder should look like this:

   ```
   .github/
   └── skills/
       ├── security-review/
       │   └── SKILL.md
       └── api-testing/
           ├── SKILL.md
           └── test-template.js    (or .py / .cs)
   ```

## Part 2 — Install a Community Plugin

Plugins are prepackaged bundles of skills, agents, hooks, and MCP servers created by the community. VS Code discovers plugins from the `github/awesome-copilot` and `github/copilot-plugins` marketplaces by default.

8. Open the Extensions view (`Ctrl+Shift+X`)
9. Type `@agentPlugins` in the search field to browse available plugins from the marketplace

   > **Alternative:** You can also install a plugin directly from a Git repository URL. Open the Command Palette (`Ctrl+Shift+P`), run **Chat: Install Plugin From Source**, and enter a repo URL (for example, `https://github.com/rwoll/markdown-review`).

10. Browse the list and pick a plugin that looks interesting or relevant to your work. Some suggestions from the `awesome-copilot` collection:
    - A code review plugin
    - A documentation plugin
    - A testing-related plugin

11. Select **Install** on the plugin you chose. If this is your first install from the marketplace, VS Code may ask you to trust the source — review and confirm.

    > **Important:** Plugins can include hooks and MCP servers that run code on your machine. Always review plugin contents before installing.

12. After installation, verify the plugin is active:
    - Open the **Agent Plugins - Installed** section in the Extensions view to confirm it appears
    - Select the gear icon in the Chat view and check **Configure Skills** — skills from the plugin should appear alongside your local ones

## Part 3 — Test Your Setup

13. Open the Copilot Chat panel
14. Type `/` to see the list of available slash commands — your `api-testing` skill should appear
15. Invoke it by typing `/api-testing for the GET /entries endpoint` and verify that:
    - Copilot uses the test template from your skill's resource file
    - The generated test follows the structure described in your `SKILL.md`
16. Also try invoking a skill or command from the plugin you installed to confirm it works

**Expected output:**
- `.github/skills/api-testing/SKILL.md` exists with `name`, `description`, and testing instructions
- The skill folder contains at least one resource file (test template) referenced from the `SKILL.md`
- A community plugin is installed and visible in the Extensions view and Configure Skills menu
- You can invoke `/api-testing` as a slash command and get a relevant response
