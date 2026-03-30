# Lab 4 — Challenge Guided: Workflow Chaining and Branch Protection

**Goal:** Make your deploy workflow depend on CI, and protect main with a branch protection rule.

**Time:** 15 minutes

**You will need:** Lab 4 Core completed (app deployed and live).

---

## Steps

### Part A: Chain CI and Deploy with `needs`

The simplest approach is to add a `needs` dependency in your deploy workflow so it only runs after CI succeeds. This requires combining both workflows into a single file.

1. Open `.github/workflows/ci.yml`
2. Add a deploy job at the bottom that depends on the CI jobs:
   ```yaml
     deploy:
       needs: [node, python, dotnet]
       if: github.ref == 'refs/heads/main'
       runs-on: ubuntu-latest
       permissions:
         pages: write
         id-token: write
       environment:
         name: github-pages
         url: ${{ steps.deployment.outputs.page_url }}
       concurrency:
         group: "pages"
         cancel-in-progress: false
       steps:
         - uses: actions/checkout@v4
         - uses: actions/configure-pages@v5
         - uses: actions/upload-pages-artifact@v4
           with:
             path: frontend
         - id: deployment
           uses: actions/deploy-pages@v4
   ```
   > **What does this achieve?** The deploy job only runs when all three CI jobs pass *and* the push is to main. On PRs, only CI runs. On merge to main, CI runs first, then deploy.

3. You'll also need to add `pages: write` and `id-token: write` to the top-level permissions in `ci.yml`, or use the job-level permissions shown above.
4. Commit and push. Verify that on merge to main, CI runs first and deploy follows.

> **Alternative approach:** If you prefer to keep CI and deploy as separate workflow files, you can use the `workflow_run` trigger in `deploy.yml`:
> ```yaml
> on:
>   workflow_run:
>     workflows: [CI]
>     types: [completed]
>     branches: [main]
> ```
> This tells the deploy workflow to run only after the CI workflow completes on main. You'd also need to add a condition to check if CI succeeded: `if: github.event.workflow_run.conclusion == 'success'`

### Part B: Add Branch Protection

5. On github.com, go to **Settings → Rules → Rulesets** (or **Branches** on older UI)
6. Click **New ruleset** (or **Add rule**)
7. Set the target to the `main` branch
8. Enable **Require status checks to pass before merging**
9. Search for and add the CI check name(s) — for example `Node.js`, `Python`, `.NET`, or `CI Passed` (if you added the summary job in Lab 3 challenge)
10. Save the ruleset
11. Test it: push a change to a branch, open a PR, and verify you can't merge until CI passes. If you introduced the intentional bug in Lab 3's challenge, you've already seen this in action — now it's enforced.

### Part C (Stretch): Azure Deploy Skeleton

If you're familiar with Azure and want to explore a different Marketplace action ecosystem:

12. Create `.github/workflows/azure-deploy.yml`:
    ```yaml
    name: Deploy to Azure

    on:
      workflow_dispatch:

    permissions:
      id-token: write
      contents: read

    jobs:
      deploy:
        runs-on: ubuntu-latest
        environment: azure-demo
        steps:
          - uses: actions/checkout@v4
          - uses: azure/login@v2
            with:
              client-id: ${{ vars.AZURE_CLIENT_ID }}
              tenant-id: ${{ vars.AZURE_TENANT_ID }}
              subscription-id: ${{ vars.AZURE_SUBSCRIPTION_ID }}
          - name: Deploy to Azure
            run: echo "Azure deployment would go here"
    ```
    > **What's different?** `azure/login@v2` is a Marketplace action published by Microsoft. It uses OIDC (OpenID Connect) — the workflow proves its identity to Azure without needing stored passwords or secrets. The `id-token: write` permission makes this possible.

13. Note: this workflow won't actually deploy (no Azure resources are set up), but it demonstrates the OIDC pattern used in enterprise deployments.

**Expected output:** Deploy only runs after CI passes. A branch protection rule on main prevents merging without green CI. *(Stretch)* An Azure deploy workflow demonstrates OIDC auth with a different Marketplace action.
