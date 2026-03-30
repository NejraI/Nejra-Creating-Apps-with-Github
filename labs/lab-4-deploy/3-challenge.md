# Lab 4 — Challenge Self-Directed: Workflow Chaining and Branch Protection

**Goal:** Make your deploy workflow depend on CI, and protect main with a branch protection rule.

**Time:** 15 minutes

**You will need:** Lab 4 Core completed (app deployed and live).

---

## Your Task

1. **Chain CI and Deploy.** Make the deploy workflow depend on CI passing. You can either combine both into a single workflow file using `needs`, or keep them as separate files and use the `workflow_run` trigger so deploy only runs after CI succeeds.
2. **Add branch protection.** Configure a branch protection rule on `main` that requires CI status checks to pass before a PR can be merged. This demonstrates how Actions integrates with repository security settings.
3. **Azure deployment skeleton.** If you have an Azure environment available: Create a `.github/workflows/azure-deploy.yml` that uses OIDC authentication with `azure/login@v2`. This explores a different set of Marketplace actions used in enterprise deployment patterns.

## Success Criteria

- Deploying only happens after CI passes (either via `needs` or `workflow_run`)
- A branch protection rule on `main` requires CI checks before merging
- A test PR cannot be merged until CI passes
- *(Stretch)* An Azure deploy workflow exists with OIDC auth configuration
