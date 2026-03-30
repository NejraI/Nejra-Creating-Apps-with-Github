# Lab 2 — Challenge Guided: Write a Test for the Summary Endpoint

**Goal:** Use Copilot to write a test that verifies your `/summary` endpoint works correctly.

**Time:** 15 minutes

**You will need:** Lab 2 Core completed (both dashboard charts working).

---

## Steps

### Part A: Create (or Open) the Test File

1. Open the test file for your language. Starter files already exist:
   - Node: `node/__tests__/summary.test.js`
   - Python: `python/test_summary.py`
   - .NET: `dotnet/Api.Tests/SummaryTests.cs`

2. Read the existing test to understand the pattern. There is already one test that checks the basic shape of the response.

### Part B: Add a Test Using Copilot

3. Place your cursor after the existing test. Write a comment describing what you want to test next. For example:

   ```
   # Test that by_tool contains the correct tool names from the data
   ```

   or:

   ```
   # Test that the verdict counts in by_verdict add up to the total
   ```

4. Press **Enter** and let Copilot suggest the test implementation. Press **Tab** to accept.

5. If you want more help, try using the `api-testing` skill: open Copilot Chat and ask `Write a test that verifies the /summary endpoint returns correct by_tool data` with the skill attached.

### Part C: Run the Test

6. Run the tests:

   > **Note:** For Node and .NET, the API server must be running in a separate terminal before you run the tests. Start it with `npm start` (Node) or `dotnet run` (.NET). Python tests use Flask's built-in test client and do not need a running server.

   ```bash
   # Node
   cd node && npm test

   # Python
   cd python && python -m pytest

   # .NET
   cd dotnet/Api.Tests && dotnet test
   ```

   > **Windows note:** If `pytest` is not recognized as a command, use `python -m pytest` instead.

7. Verify all tests pass.

### Part D: Optional — Harden the POST Endpoint

8. Select the code for your `POST /entries` endpoint in your API file.
9. Open Copilot Chat and ask: *"Review this endpoint for input validation issues. What edge cases are not handled?"*

   > **Tip:** Attach the security review skill you built in Lab 1 for more structured output.

10. Review what Copilot finds. Common gaps include:
    - No validation that `verdict` is one of the allowed values (`Faster`, `Same`, `Slower`, `Surprising`)
    - No string length limits (a user could submit megabytes of text)
    - No input sanitization (XSS risk if values are rendered in HTML)

11. Apply the fixes that make sense to you.
12. Test in the browser — try submitting an entry with an invalid verdict or very long strings.

### Part E: Optional — Compare with Coding Agent

13. If your instructor started a coding agent session earlier, check if a PR has been created in your repo.
14. Compare the agent's implementation of `/summary` with your own — what is different? What is similar?

**Expected output:** At least one passing test that verifies the `/summary` endpoint response shape and data. Optionally, improved validation on the POST endpoint.

You've built a feature, seen it work in the UI, and tested it — that's a solid engineering workflow!
