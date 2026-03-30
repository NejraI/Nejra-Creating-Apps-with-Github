---
name: api-testing
description: "Write tests for the AI Experiment Log API. Use this skill when writing, scaffolding, or reviewing tests for API endpoints."
---

# API Testing

When writing tests for the AI Experiment Log API:

1. Use the test template as a starting point:
   - dotnet: [TestTemplate.cs](./TestTemplate.cs)
2. Place test files in the appropriate folder for the language:
   - dotnet: `dotnet/Api.Tests/`
3. Each test should: arrange test data, act by calling the endpoint, assert the expected result
4. Test both the happy path and at least one error case (e.g., missing ID returning 404)
5. The API reads from `/data/experiments.json` — tests can use this data or mock it