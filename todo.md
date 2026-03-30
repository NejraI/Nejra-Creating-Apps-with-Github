# Workshop Preparation Checklist

Organized by category, roughly in the order you should tackle them. Items that block other items are marked **[BLOCKER]**.

---

## 1. Verify tool availability and licensing (do this first, it affects everything else)

- [V] **[BLOCKER]** Confirm your GitHub Spark license is active and Spark is accessible at the current date -- do not assume
- [V] Check the VS Code stable release notes for agent skills support landing in stable (last confirmed: Insiders only, December 2025) -- adjust Lab 1 challenge tier accordingly
- [V] Verify the current Copilot free tier limits are still 2,000 completions and 50 premium requests (check docs.github.com/en/copilot/get-started/plans)
- [V] Confirm Copilot CLI is available to all plan tiers (confirmed GA February 2026, but verify nothing has changed)
- [V] Check that the 30-day Copilot Pro trial is still offered and find the exact signup URL for the repo README
- [V] Verify the `github/awesome-copilot` community collection is still the correct reference URL
- [V] Confirm agent mode in VS Code stable is available on the free tier
- [V] Check the GitHub Codespaces free tier limits for attendees on free GitHub accounts (monthly included hours)

---

## 2. Azure (even though attendees deploy to Pages, you demo Azure)

- [v] **[BLOCKER]** Pull up the current Azure docs for GitHub Actions OIDC authentication and document exactly where the errors are -- capture screenshots dated to your prep day so you have a concrete teaching moment even if Microsoft fixes them before the workshop
- [v] Set up a personal Azure subscription or demo environment for the OIDC demo
- [v] Configure OIDC federation between your demo repo and that Azure subscription (follow the correct steps, not the docs)
- [v] Test the `azure/login` action with OIDC end-to-end at least twice before the day
- [v] Write down the correct subject claim format for branch-based deployment as a reference card for the session
- [v] Decide what Azure resource type you are showing (Container Apps, App Service, or Static Web Apps) and have it pre-provisioned

---

## 3. Build the workshop repo

- [V] **[BLOCKER]** Create the repo with the three language folders (node/, python/, dotnet/) and shared frontend
- [ ] **[BLOCKER]** Define the app concept specifically enough to build the starter -- the "Hype vs. Reality Tracker" concept from earlier sessions is the candidate; confirm it or replace it
- [ ] Implement a working baseline in all three language folders (the thing attendees run in Lab 0)
- [ ] Implement the feature that attendees will add in Lab 2 (you need the answer to write the guided lab instructions)
- [ ] Build the shared frontend SPA that connects to the language-specific APIs
- [ ] Verify the app works end-to-end in all three language implementations before writing any lab instructions
- [ ] Create `.devcontainer/devcontainer.json` that supports all three languages in one Codespace
- [ ] Add a one-command local setup script for the VS Code fallback path and document it in the README
- [ ] Test a cold Codespace start from a fresh fork -- time it, and check whether simultaneous starts from multiple accounts slow it down
- [ ] Add the pre-event email instructions to the repo README (GitHub account, Git basics, Copilot setup, Codespace pre-warm instruction)
- [ ] Make the repo public, or confirm the sharing mechanism for attendees

---

## 4. Copilot configuration files (ship these in the repo)

- [ ] Write `/.github/copilot-instructions.md` as a worked example attendees can use or modify
- [ ] Write `/.github/prompts/feature.prompt.md` as the example for Lab 1
- [ ] Write `/.github/prompts/test.prompt.md` as the extra challenge example for Lab 1
- [ ] Create `/.github/skills/<skill-name>/` with a worked skill example for the demo
- [ ] Test all three files in a real Copilot session and verify they produce meaningfully better output than an equivalent unguided prompt
- [ ] Test the skill via Copilot CLI and confirm the invocation works end-to-end

---

## 5. GitHub Actions workflows

- [ ] **[BLOCKER]** Write the CI workflow (`.github/workflows/ci.yml`) with language matrix support for all three folders
- [ ] **[BLOCKER]** Write the deploy workflow (`.github/workflows/deploy.yml`) targeting GitHub Pages
- [ ] Test both workflows on a real fork with each language folder, not just one
- [ ] Verify the Pages deployment produces a live URL with the SPA working (not a blank page or 404)
- [ ] Verify the matrix build runs correctly for Node 18/20/22, Python 3.11/3.12, and .NET 8/9
- [ ] Write the broken CI example for the Extra Challenge (a version of the workflow that fails in a diagnosable way)
- [ ] Write the OIDC Azure deployment workflow as a reference file in the repo even if attendees do not run it
- [ ] Test the Azure workflow end-to-end with your demo environment

---

## 6. Lab documents

For each lab (0, 1, 2, 3) write four documents:

- [ ] Lab 0: Core Guided
- [ ] Lab 0: Core Self-directed
- [ ] Lab 0: Extra Challenge Guided
- [ ] Lab 0: Extra Challenge Self-directed
- [ ] Lab 1: Core Guided
- [ ] Lab 1: Core Self-directed
- [ ] Lab 1: Extra Challenge Guided
- [ ] Lab 1: Extra Challenge Self-directed
- [ ] Lab 2: Core Guided
- [ ] Lab 2: Core Self-directed
- [ ] Lab 2: Extra Challenge Guided
- [ ] Lab 2: Extra Challenge Self-directed
- [ ] Lab 3: Core Guided
- [ ] Lab 3: Core Self-directed
- [ ] Lab 3: Extra Challenge Guided
- [ ] Lab 3: Extra Challenge Self-directed

For each guided version: verify that following the steps exactly, from a clean fork, produces the expected output. Do this yourself before the workshop.

- [ ] Put all 16 documents in the repo so attendees access them from their Codespace without switching windows
- [ ] Write the one-page Git cheat sheet referenced in the account check (clone, commit, push, pull, branch, PR)
- [ ] Write the issue template used in the pre-lab exercise before Lab 2
- [ ] Write the feature issue and non-functional issue examples (one bad, one good) for the Issues explanation

---

## 7. Demos to prepare and test

Each demo should be rehearsed at least twice and have a fallback plan.

- [ ] **Copilot tiers demo:** three-approach comparison (naive chat / instructions-grounded inline / prompt-file chat) -- pick the specific task now so it fits in 10 minutes and the output difference is visually obvious
- [ ] **Skills demo:** skill invocation via Copilot CLI, comparison to plain prompt -- rehearse the exact commands
- [ ] **Agentic workflow demo (agent mode):** pick a specific multi-file task, rehearse the full session including the mistake and correction moment
- [ ] **Agentic workflow demo (coding agent):** pick a specific issue, confirm timing -- the PR needs to arrive during Lab 2's Extra Challenge, so test how long it takes end-to-end
- [ ] **PRs demo:** prepare the specific bad PR and good PR examples, not generic ones -- they should relate to the workshop app so they feel relevant
- [ ] **Spark demo:** pick the specific app you will build live, rehearse it, record a screen capture fallback
- [ ] **Azure authentication demo:** rehearse the exact steps you will show, including the wrong docs and the correct alternative -- this needs to be tight at 5 minutes
- [ ] Verify all demos work in the environment you will present from (your laptop, the conference wifi, the Codespace)

---

## 8. Pre-event logistics

- [ ] Write and send the pre-event email covering: create GitHub account, verify Git is installed, enable Copilot (free or trial), consider pre-warming a Codespace the night before
- [ ] Confirm whether the conference provides wifi credentials in advance -- test if GitHub Codespaces is accessible on conference networks (some block websocket traffic that Codespaces requires)
- [ ] Prepare a hotspot fallback for yourself in case conference wifi fails during demos
- [ ] Find out the room setup: how many attendees maximum, screen visibility from the back, whether attendees can see both their screen and yours simultaneously
- [ ] Confirm whether attendees bring laptops (almost certainly yes, but verify)
- [ ] Prepare a shortened URL or QR code pointing to the workshop repo for the opening slide

---

## 9. Run-throughs

- [ ] Do one complete dry run of the full day alone, following your own lab instructions from a clean fork, timing each section
- [ ] Adjust timing table based on actual times, not estimated times
- [ ] Do at least one rehearsal of the Codespaces Lab 0 flow specifically -- this is the highest-risk lab and the one that has to work for everyone
- [ ] Find one or two volunteers to run Lab 2 and Lab 3 against the guided instructions and report where they got stuck
- [ ] Fix all blockers found in the dry run before the workshop date

---

## 10. Day-of preparation

- [ ] Start the coding agent session for the agentic demo 30-60 minutes before the session, or confirm the exact timing so the PR arrives during Lab 2
- [ ] Have all demo scripts open in separate tabs before the day starts
- [ ] Have Spark recorded fallback ready and accessible offline
- [ ] Have the Azure environment pre-provisioned and warm
- [ ] Have the broken CI example branch ready to show but not yet open
- [ ] Print or have digital access to the workshop schedule with timing visible to you during the day
- [ ] Check the Copilot status page (githubstatus.com) the morning of the event