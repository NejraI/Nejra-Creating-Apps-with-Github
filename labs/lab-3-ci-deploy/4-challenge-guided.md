# Lab 3 — Challenge Guided: More Jobs, Linting, Dependencies, and Breaking CI

**Goal:** Add CI jobs for the other languages, enable linting, add a summary job that depends on all language jobs, and demonstrate that CI catches deliberate errors.

**Time:** 15 minutes

**You will need:** Lab 3 Core completed (CI green on your PR).

---

## Steps

### Part A: Add Jobs for the Remaining Languages

1. Open `.github/workflows/ci.yml`
2. You already have one job from the core lab. Add jobs for the other two languages following the same pattern. The steps are listed in `ci.yml` — wrap them in the same `runs-on`, `defaults` with `working-directory`, and `steps` structure you used for your first job.

   For reference, here are all three jobs together:

   ```yaml
   jobs:
     node:
       name: Node.js
       runs-on: ubuntu-latest
       defaults:
         run:
           working-directory: node
       steps:
         - uses: actions/checkout@v4
         - uses: actions/setup-node@v4
           with:
             node-version: "20"
         - run: npm ci
         - run: npm test

     python:
       name: Python
       runs-on: ubuntu-latest
       defaults:
         run:
           working-directory: python
       steps:
         - uses: actions/checkout@v4
         - uses: actions/setup-python@v5
           with:
             python-version: "3.12"
         - run: pip install -r requirements.txt
         - run: pytest --tb=short

     dotnet:
       name: .NET
       runs-on: ubuntu-latest
       defaults:
         run:
           working-directory: dotnet
       steps:
         - uses: actions/checkout@v4
         - uses: actions/setup-dotnet@v4
           with:
             dotnet-version: "8.0.x"
         - run: dotnet restore
         - run: dotnet build --no-restore
         - run: dotnet build Api.Tests --no-restore
         - run: dotnet test Api.Tests --no-build
   ```

3. Commit and push. Verify all three jobs run in the Actions UI.

### Part B: Add a Lint Step

1. Open `.github/workflows/ci.yml`
2. Uncomment or add the lint step for your language:
   - **Node:** `npx eslint .` — you will also need to create `node/.eslintrc.json` with this content:
     ```json
     {
       "env": {
         "node": true,
         "es2021": true,
         "jest": true
       },
       "extends": "eslint:recommended",
       "parserOptions": {
         "ecmaVersion": "latest"
       }
     }
     ```
   - **Python:** `pip install ruff && ruff check .`
   - **.NET:** Build warnings are checked during `dotnet build`
3. Fix any lint errors and push again

### Part C: Add a Summary Job with Dependencies

4. Add a new job at the bottom of `ci.yml` that only runs when all three language jobs succeed:
   ```yaml
   ci-passed:
     name: CI Passed
     runs-on: ubuntu-latest
     needs: [node, python, dotnet]
     steps:
       - run: echo "All CI checks passed"
   ```
   > **What does `needs` do?** It creates a dependency — `ci-passed` will only run after `node`, `python`, and `dotnet` all complete successfully. If any of them fails, this job is skipped. This is useful for requiring a single status check on PRs instead of checking each language individually.
5. Commit and push. In the Actions UI, notice how the `ci-passed` job appears after the three language jobs and only runs once they all succeed.

### Part D: Intentional Bug

6. Create a new branch for this experiment:
   ```bash
   git checkout -b break-ci
   ```
7. Introduce a deliberate syntax error in your API file (e.g., remove a closing bracket)
8. Commit and push to the branch:
   ```bash
   git add -A
   git commit -m "Introduce deliberate bug"
   git push origin break-ci
   ```
9. Open a PR from `break-ci` into `main` and verify that CI catches the error — the check should fail
10. Fix the bug, push again, verify CI goes green
11. **Close this PR without merging** — its purpose is to demonstrate that CI protects main

**Expected output:** Lint is active, a summary job depends on all language jobs via `needs`, and you have demonstrated that CI catches bugs before they reach main.
