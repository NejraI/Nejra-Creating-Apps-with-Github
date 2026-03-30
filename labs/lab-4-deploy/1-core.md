# Lab 4 — Core Self-Directed: Deploy with GitHub Actions & the Marketplace

**Goal:** Add the deployment steps to the deploy workflow, discover how GitHub Marketplace actions simplify complex tasks, and get your app live on GitHub Pages.

**Time:** 30 minutes

**You will need:** Lab 3 completed (CI green on your open PR).

---

## Your Task

In Lab 3 you learned how workflow structure works — `name`, `on`, `jobs`, `runs-on`, `defaults`. The CI steps were given and you wrapped them in that structure. Now you flip it: the deploy workflow already has its structure (environment, permissions, concurrency), and your job is to figure out the right steps. Specifically, which Marketplace actions to use for deploying to GitHub Pages.

Your job:

1. **Configure GitHub Pages.** Set the Pages source to GitHub Actions in your repo settings
2. **Insert the Marketplace actions.** Make use of the following actions: `actions/configure-pages`, `actions/upload-pages-artifact`, and `actions/deploy-pages`. 
3. **Add a push trigger** so the workflow runs automatically when code is pushed to main
4. **Merge the PR.** This triggers the deploy workflow. CI (Lab 3) validated the code, deploy (Lab 4) ships it.
5. **Verify your live URL.** Find it in Settings → Pages and visit it. 

## Success Criteria

- Deploy workflow starts with Marketplace actions for the deployment
- The `permissions`, `environment`, and `concurrency` settings make sense to you
- Deploy workflow runs automatically on push to main
- You have visited a Marketplace action page and understand action versioning
- Your Lab 3 PR is merged and the deploy workflow ran
- Your frontend is live at `https://<username>.github.io/<repo>/`
- The Experiment Log shows entries and the Dashboard displays charts (loaded from the bundled `experiments.json` static data)
