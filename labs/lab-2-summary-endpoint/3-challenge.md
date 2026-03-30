# Lab 2 — Challenge Self-Directed: Write a Test for the Summary Endpoint

**Goal:** Use Copilot to write a test that verifies your `/summary` endpoint returns the correct data.

**Time:** 15 minutes

**You will need:** Lab 2 Core completed (both dashboard charts working).

---

## Your Task

Write at least one test for the `/summary` endpoint using your language's test framework. Use Copilot to help — try attaching the `api-testing` skill as context.

The test should verify that the endpoint returns the expected shape: `total`, `by_verdict`, and `by_tool`.

**Bonus:** Use the security review skill you built in Lab 1 to review your `POST /entries` endpoint. Ask Copilot to check for validation gaps — what does it find? Apply the fixes it suggests.

## Success Criteria

- At least one test file exists and passes (`npm test`, `python -m pytest`, or `dotnet test`)
- The test checks for the three top-level keys (`total`, `by_verdict`, `by_tool`)
- The test verifies that the data types are correct (e.g., `total` is a number)
- *(Bonus)* POST endpoint has improved input validation based on the security review
