# Lab 3 — Core Guided: CI Workflow Structure and Triggers

**Goal:** Take the pre-written CI steps and wrap them in a working workflow — add the workflow name, triggers, job definitions, and structural YAML that GitHub Actions needs to run them.

**Time:** 30 minutes

**You will need:** Lab 2 completed (or copied the rescue file from `rescue/` — see the [rescue README](../../rescue/README.md)), changes committed and pushed.

---

## Steps

### Part A: Read the CI Workflow File

1. Open `.github/workflows/ci.yml` in your Codespace
2. Read through the file. Notice:
   - The **steps** for each language (checkout, setup, build/test commands) are already listed in comments — they're complete and ready to use
   - The file has an `on: workflow_dispatch` trigger (manual only) but **no workflow name**
   - There are **no `jobs:` defined** — the steps are just sitting in comments with no structure around them
3. Your task is to add the structural YAML that turns these steps into a working workflow

### Part B: Understand the Workflow Structure

Before you start, here's how a GitHub Actions workflow is structured — these are the pieces you need to add:

- **`name:`** — the display name shown in the Actions UI tab. Goes at the top of the file.
- **`on:`** — defines when the workflow runs. `workflow_dispatch` means manual. You'll add `push` and `pull_request` triggers.
- **`jobs:`** — contains one or more named jobs. Each job runs on its own runner.
  - **`runs-on:`** — which virtual machine image to use (always `ubuntu-latest` for this workshop)
  - **`defaults: run: working-directory:`** — sets the default folder for all `run:` steps so commands execute in the right language folder
  - **`steps:`** — the sequence of actions (`uses:`) and shell commands (`run:`) the job executes

The steps themselves are already written. You're building the container that holds them.

### Part C: Add the Workflow Name

4. At the top of the file (after the comment block), add a name:
   ```yaml
   name: CI
   ```
   > This is what appears in the Actions tab on GitHub. Without it the workflow shows the filename.

### Part D: Add Triggers

5. Find the `on:` section — currently it only has `workflow_dispatch`
6. Change it to also trigger on push to main and on pull requests:
   ```yaml
   on:
     workflow_dispatch:
     push:
       branches: [main]
     pull_request:
       branches: [main]
   ```
   > **Why three triggers?** `workflow_dispatch` lets you run it manually (useful for testing). `push` runs CI when code lands on main. `pull_request` runs CI on PR branches so you see the result *before* merging — this is what makes CI a safety net.
7. Save the file

### Part E: Create a Job for Your Language

Now wrap the steps for your chosen language in a proper job definition. The steps are listed in the comments — uncomment them and add the structure around them.

8. Add a `jobs:` section and define a job for your language. Use Copilot to help — describe what you want in a comment and let it suggest the YAML.

   **Node.js:**
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
   ```

   **Python:**
   ```yaml
   jobs:
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
   ```

   **.NET:**
   ```yaml
   jobs:
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

9. Save the file

> **What you just built:** Look at the structure you added vs. the steps that were already there. The steps (`uses:` and `run:` lines) do the actual work. Everything else — `name`, `on`, `jobs`, `runs-on`, `defaults` — is the GitHub Actions structure that controls *when*, *where*, and *how* those steps execute. That structure is what this lab is about.

> **Tip:** YAML indentation matters. Each nested level is 2 spaces. The `steps:` list items start with `- ` and must all be at the same indentation level within their job. If the workflow fails with a syntax error, check indentation first.

### Part F: Create a PR and Watch CI

10. Create a new branch:
    ```bash
    git checkout -b enable-ci
    ```
11. Commit the workflow changes:
    ```bash
    git add .github/workflows/ci.yml
    git commit -m "Add CI workflow structure and triggers"
    ```
12. Push the branch:
    ```bash
    git push origin enable-ci
    ```
13. On github.com, open a Pull Request from `enable-ci` into `main`
14. Watch the CI workflow run in the PR checks — you should see a yellow "pending" indicator turn green (or red)

### Part G: Explore the Actions UI

15. Click **Details** next to the CI check in your PR to open the workflow run
16. Explore the Actions UI:
    - **Jobs list** (left sidebar) — see your job and its status
    - **Step log** — click the job, then expand individual steps to see their output
    - **Annotations** — errors and warnings appear as annotations on the workflow summary
    - **Re-run** button — you can re-run failed jobs without pushing again
17. If the job fails, read the error in the step log. Common issues:
    - YAML indentation errors — steps not aligned, or `defaults` nested incorrectly
    - Missing `runs-on` — the job doesn't know which runner to use
    - Wrong `working-directory` — commands run in the wrong folder
18. Fix the issue, commit, and push again:
    ```bash
    git add -A
    git commit -m "Fix CI issue"
    git push
    ```
19. **Once CI is green, stop here.** Do not merge the PR yet — deploying to a live URL is the goal of Lab 4.

**Expected output:** Your CI workflow has a name, triggers on push and PR, and a properly structured job that wraps the pre-written steps. CI runs automatically and your PR shows a green check. The PR remains open for Lab 4.
