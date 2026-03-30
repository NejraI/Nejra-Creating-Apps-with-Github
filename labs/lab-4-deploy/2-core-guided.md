# Lab 4 — Core Guided: Deploy with GitHub Actions & the Marketplace

**Goal:** Add the deployment steps to the deploy workflow, discover how GitHub Marketplace actions simplify complex tasks, and get your app live on GitHub Pages.

**Time:** 30 minutes

**You will need:** Lab 3 completed (CI green on your open PR).

---

In Lab 3 you learned workflow structure — the CI steps were given and you wrapped them in `name`, `on`, `jobs`, `runs-on`, and `defaults`. Now you flip it: the deploy workflow already has its structure in place, and your job is to **figure out the right steps** — which Marketplace actions to use for deploying to GitHub Pages.

---

## Steps

### Part A: Set Up GitHub Pages

1. In your fork on github.com, go to **Settings → Pages**
2. Under **Source**, select **GitHub Actions**
3. This tells GitHub to use your deploy workflow rather than serving from a branch

### Part B: Start with Basic `run` Steps

4. Open `.github/workflows/deploy.yml` in your Codespace
5. Take a look at what's already in the file — the job has `environment`, `permissions`, and `concurrency` settings, but the actual steps are just a placeholder `echo`
6. Replace the placeholder step with a checkout and a basic `run` step:
   ```yaml
       steps:
         # Check out the repository
         - uses: actions/checkout@v4

         # List the frontend files to verify checkout worked
         - run: ls -la frontend/
   ```
7. Save the file
8. Add a push trigger so the workflow runs automatically:
   ```yaml
   on:
     workflow_dispatch:
     push:
       branches: [main]
   ```
9. Save again

> **What you've built so far:** A working workflow that checks out code and runs a shell command. This is the same pattern as CI — `uses:` for a pre-built action, `run:` for a shell command. But it doesn't actually deploy anything yet.

### Part C: Replace with Marketplace Actions

Now swap your basic `run` step for purpose-built actions from the GitHub Marketplace. These are actions that other developers (including GitHub themselves) have built and published.

10. Replace the `ls` step with the three Marketplace actions needed for Pages deployment:
    ```yaml
        steps:
          # Check out the repository
          - uses: actions/checkout@v4

          # Configure GitHub Pages settings
          - name: Setup Pages
            uses: actions/configure-pages@v5

          # Upload the frontend folder as the Pages artifact
          - name: Upload artifact
            uses: actions/upload-pages-artifact@v4
            with:
              path: frontend

          # Deploy to GitHub Pages
          - name: Deploy to GitHub Pages
            id: deployment
            uses: actions/deploy-pages@v4
    ```
11. Save the file

> **Why Marketplace actions?** Compare what you had before (a shell `ls` command) with what these actions do:
> - `configure-pages` — calls the GitHub Pages API to prepare your site settings
> - `upload-pages-artifact` — packages the `frontend/` folder into a deployment artifact with the right format
> - `deploy-pages` — triggers the actual Pages deployment and waits for it to complete
>
> You *could* script all of this with `curl` and shell commands, but these actions handle authentication, retries, error messages, and API versioning for you. That's the value of the Marketplace.

> **Why `id: deployment`?** The `environment` block at the top of the job references `${{ steps.deployment.outputs.page_url }}` — this ID connects the deploy step to the environment URL that appears in the Actions summary.

### Part D: Understand the Workflow Scaffold

Before moving on, take a moment to understand the parts of `deploy.yml` that were already in place. These are Actions concepts you haven't worked with until now:

12. Look at the **`permissions`** block near the top:
    ```yaml
    permissions:
      contents: read
      pages: write
      id-token: write
    ```
    This limits what the workflow's `GITHUB_TOKEN` can do. `pages: write` allows deploying to Pages. `id-token: write` allows the workflow to prove its identity (used by the deploy action). This is a security best practice — only grant what's needed.

13. Look at the **`environment`** setting in the job:
    ```yaml
    environment:
      name: github-pages
      url: ${{ steps.deployment.outputs.page_url }}
    ```
    This connects the job to a GitHub Environment. After deployment, the URL appears in the Actions summary and in the repo's **Environments** tab. Environments can also have protection rules (like requiring approval before deploy).

14. Look at the **`concurrency`** group:
    ```yaml
    concurrency:
      group: "pages"
      cancel-in-progress: false
    ```
    This ensures only one Pages deployment runs at a time. If you push twice quickly, the second run waits for the first to finish rather than deploying in parallel and causing conflicts.

### Part E: Explore the GitHub Marketplace

15. Open your browser and go to the GitHub Marketplace: https://github.com/marketplace
16. Search for one of the actions you used, for example `actions/deploy-pages`
17. On the action's page, notice:
    - The **README** — usage examples, inputs, outputs
    - The **version tags** — `@v4` means major version 4. You can also pin to a specific commit SHA for maximum security
    - The **"Verified creator"** badge — `actions/` actions are published by GitHub themselves
18. Understanding versioning: `uses: actions/deploy-pages@v4` means "use the latest v4.x release". If the action gets a bug fix (v4.0.1 → v4.0.2), you get it automatically. A breaking change would be v5, which you'd have to opt into.

### Part F: Merge and Deploy

19. Commit the deploy workflow changes to your existing branch (or create a new one):
    ```bash
    git add .github/workflows/deploy.yml
    git commit -m "Add Pages deployment workflow"
    git push
    ```
20. If your Lab 3 PR is still open, it now includes this commit too. If you need a new PR, open one.
21. Wait for CI to go green, then **merge the PR**
22. Go to the **Actions** tab and watch the deploy workflow run — it triggers on the push to main from the merge
23. Once complete, find your live URL:
    - **Settings → Pages** shows the URL (typically `https://<username>.github.io/<repo>/`)
    - **Code tab → Environments** (right sidebar) shows the `github-pages` environment with deployment history
24. Visit the URL and verify your app is live — the Experiment Log should show entries, and the Dashboard should display charts

**Expected output:** Your frontend is deployed to GitHub Pages automatically on merge to main. The Experiment Log and Dashboard are accessible at your live URL and display data from the bundled `experiments.json` file. The Environments tab shows your deployment history.

> **How does this work without a backend?** The `frontend/` folder includes a copy of `experiments.json` (the experiment data). The JavaScript in `app.js` tries to contact the API first. When it cannot reach a backend (as on GitHub Pages), it falls back to loading `experiments.json` as a static file. This means the Log and Dashboard display real data even on the static site. The **Add Entry** form will not work on Pages, since there is no server to save new entries — this is expected for a static deployment.
