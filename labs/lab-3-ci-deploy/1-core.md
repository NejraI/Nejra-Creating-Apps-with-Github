# Lab 3 — Core Self-Directed: CI Workflow Structure and Triggers

**Goal:** Take the pre-written CI steps and wrap them in a working workflow — add the workflow name, triggers, job definitions, and structural YAML that GitHub Actions needs to run them.

**Time:** 30 minutes

**You will need:** Lab 2 completed (or copied the rescue file from `rescue/` — see the [rescue README](../../rescue/README.md)), changes committed and pushed.

---

## Your Task

The CI workflow file (`.github/workflows/ci.yml`) has the steps already written: Checkout, setup, build, and test commands for each language are listed and ready to use. What's missing is the **structure** that makes it a real workflow. Right now the file won't run because the steps aren't wrapped in proper jobs.

Your job is to learn how workflow YAML is structured and build that structure around the existing steps. Make sure to use Copilot!

1. **Read the workflow file.** Open `ci.yml` and find the steps listed for each language. These are the actions and commands that do the actual work. They're complete, you don't need to change them.
2. **Add a workflow name.** Every workflow needs a name at the top level. This is what appears in the Actions UI.
3. **Add triggers.** Change the `on:` section so the workflow runs automatically on `push` to main and on `pull_request`, not just on manual `workflow_dispatch`. 
4. **Create a job for your chosen language.** Define a `jobs:` section with a job that has:
   - `runs-on:` tells Actions which runner to use
   - `steps:` copy in the steps for your language from the comments
5. **Create a branch, commit, and push.** Your workflow changes.
6. **Open a Pull Request.** Watch CI run as a PR status check. Make sure it succeeds for your language.

## Success Criteria

- The workflow has a `name:` that appears in the Actions UI
- The `on:` trigger includes `workflow_dispatch`, `push` (to main), and `pull_request`
- At least one job is defined with `runs-on:` and the correct `steps:` for your language
- The existing steps (checkout, setup, build/test commands) are properly placed inside the job structure
- CI runs automatically when you push to your branch / open the PR
- You can navigate the Actions UI: find the workflow run, read job logs, see step output
- Your PR shows a green CI check (at minimum for your chosen language)
- The PR is not merged yet that happens in Lab 4
