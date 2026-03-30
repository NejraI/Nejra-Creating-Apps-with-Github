Good structural call on the combined lab. Moving Spark earlier frees the end of the day for the result moment. Here is the full revised schedule with the new lab format explained and applied throughout.

---

## Lab format (apply this consistently, explain it once at 09:00)

Every lab has this shape:

**Core** -- what everyone should complete. Available in two modes:
- **Guided:** numbered steps, every action spelled out, expected output shown
- **Self-directed:** a goal statement and constraints only, no steps

**Extra Challenge** -- for people who finish Core early. Also available in both modes:
- **Guided:** same step-by-step format as Core guided
- **Self-directed:** goal and constraints only

Participants pick their mode at the start of each lab and can switch between them at any point. The guided and self-directed versions produce identical outcomes -- the difference is how much they figure out themselves.

Make this visible physically: print both versions of each lab, or put them on separate pages in the digital handout. Do not make participants hunt for their version.

---

# Full-Day Schedule

**09:00 - 16:30**
**Through-line:** one repo, one growing app, shipped by end of day
**Environment:** Codespaces primary, local VS Code fallback

> Quick-reference version: [schedule.md](schedule.md)

---

## 09:00 - 09:20 | Welcome, account check, deployment decision

**Mode:** facilitated, interactive

### GitHub and Git check (5 min)

Show of hands, three questions:

1. GitHub account active and logged in right now?
2. Comfortable with clone, commit, push, pull?
3. Copilot enabled (free or paid)?

Note the Copilot split. Address it directly: "If you are on the free tier, you have 50 chat messages for the whole month. We have designed the labs so Core is completable with zero chat messages -- inline completions only. Chat is an enhancement. If you want more for today, GitHub offers a 30-day free Pro trial -- the link is in the repo README. Do that now if you want to use chat freely today."

Have a pre-event email covering all three so this is confirmation, not rescue.

### Workshop map (5 min)

Walk the flow: **Idea → Code → PR → CI + Deploy → Maintain**

Show the repo. Introduce the app concept briefly. Architecture explanation comes after Lab 0.

### Deployment decision (5 min)

No poll this time -- everyone deploys to GitHub Pages. State this clearly. "At the end of the day, everyone will have a live URL on GitHub Pages. I will also demo how Azure deployment works and explain the authentication pattern, because that is what production looks like. You will understand both."

---

## 09:20 - 10:15 | Lab 0: Codespaces (55 min)

**Lab first, explain after.**

### Core -- Guided

1. Go to the workshop repo URL (on screen)
2. Click Fork, accept defaults
3. Click Code > Codespaces > Create codespace on main
4. Wait for the Codespace to open (fast -- lightweight base image with no language runtimes)
5. Open `.devcontainer/devcontainer.json` and add your chosen language: the dev container feature, VS Code extension, postCreateCommand, and forwardPorts entry (reference table is in the lab document)
6. Rebuild the container (Command Palette > Rebuild Container)
7. In the terminal, run the start command for your language
8. Click the Ports tab, click the globe icon next to the forwarded port
9. You should see the running app in a browser tab

Expected output: app visible in browser, devcontainer customized for one language.

If Codespaces fails at any step: clone the repo locally, run the one-command setup in the README, open in VS Code.

### Core -- Self-directed

Get the workshop repo running in a browser tab using Codespaces. The Codespace starts with a minimal base image -- you need to add your chosen language (Node, Python, or .NET) to the dev container configuration, rebuild, and run the app. Use the reference table in the lab document and the `.devcontainer` folder as starting points.

### Extra Challenge -- Guided

1. In the Codespace, open `.devcontainer/devcontainer.json`
2. Add the Azure CLI as a dev container feature
3. Open the command palette, run "Rebuild Container"
4. Verify the Azure CLI is installed with `az --version`
5. Add a VS Code extension you actually use
6. Rebuild and verify it appears in the Extensions sidebar
7. Add a VS Code setting to hide the minimap, rebuild, and verify

### Extra Challenge -- Self-directed

Modify the devcontainer to add a CLI tool, an extension, and a setting beyond your language setup. Understand the relationship between the three language folders and the shared frontend before the debrief.

### Checkpoint (5 min, hard stop)

Everyone with a running app? Blockers? Resolve before moving on.

### Debrief and architecture (15 min)

Now explain what they just ran:

Easy: what a Codespace is, what a devcontainer does (a version-controlled development environment, comparable to a Dockerfile for your editor), when to use it vs local.

Hard: billing model (compute hours, free tier limits), how devcontainers change team onboarding, the architecture of the app (frontend SPA + language-specific API layer + shared data, same behavior in three languages).

---

## 10:15 - 10:35 | Break

---

## 10:35 - 11:05 | Copilot: tiers, limits, and working within them (30 min)

**Mode:** explanation + demo

Cover the tier structure honestly. Key numbers: free tier gets 2,000 inline completions and 50 premium requests per month. All chat counts as premium. Inline completions and context files (instructions, prompt files) cost nothing beyond the completion itself.

The spectrum from completion to agentic:
- Inline completion: Copilot suggests as you type, you accept or reject. Free, fast, no context window overhead.
- Chat: you ask, Copilot answers. Uses premium requests. Quality depends heavily on context.
- Agent mode (VS Code): Copilot plans and executes multi-file tasks autonomously. Uses premium requests, can use many in one session.
- Coding agent (async): Copilot works in the background, opens a PR. Requires Pro. Uses a separate premium request pool.

**Demo (10 min):** same task, three approaches:

1. Naive chat prompt with no context. One premium request consumed, mediocre output.
2. Inline completion with `copilot-instructions.md` in place. No extra cost beyond the completion. Visibly better output.
3. Prompt file-grounded chat. One premium request, much better output because context is precise.

Show the usage counter in GitHub settings so people know how to monitor their own consumption.

---

## 11:05 - 12:00 | Lab 1: Custom instructions, prompt files, and skills demo (55 min)

**Learning theme:** configuration that makes Copilot work for your codebase, not against it

### Explain (10 min)

**Custom instructions** (`/.github/copilot-instructions.md`): always-on, applied to every Copilot interaction in this repo. Zero extra cost -- it is just context injected alongside each request.

**Prompt files** (`/.github/prompts/*.prompt.md`): reusable, manually invoked prompts for specific task types. You call them explicitly. Costs one premium request when used in chat, but the output quality improvement justifies it. Can also be used as a reference for inline completions without triggering a chat request.

**Skills** (`.github/skills/<skill-name>/`): folders with instructions, scripts, and resources that Copilot loads automatically when relevant. Work across Copilot coding agent and Copilot CLI. VS Code stable support is in progress -- you will demo this rather than have participants do it hands-on, because the stable VS Code release date is not confirmed. The concept transfers directly.

**Skills demo (5 min):** show a skill in the repo, show it being invoked in Copilot CLI, show the difference in output compared to a plain prompt. Point to `github/awesome-copilot` as the community collection to explore after the workshop.

### Lab (45 min)

### Core -- Guided

1. In the repo root, create the file `/.github/copilot-instructions.md`
2. Add these sections to the file:
   - `## Language` -- specify which language folder you are working in today
   - `## Code style` -- two to three rules (e.g., "use async/await, not callbacks", "add a JSDoc comment to every exported function")
   - `## Testing` -- one rule (e.g., "always suggest a test alongside any new function")
3. Save the file. Copilot picks this up immediately, no restart required.
4. Create the folder `/.github/prompts/`
5. Create the file `/.github/prompts/feature.prompt.md`
6. Add a prompt that describes how to implement a new API endpoint in your chosen language, including the expected input/output format and where the file should live
7. Open one of the existing API files in your language folder
8. Use Copilot inline completion (Tab, not Chat) to add a comment explaining what the file does -- watch how the instructions file shapes the output
9. Use the prompt file as a reference while writing a new small function using inline completion only

Expected output: instructions file and at least one prompt file committed to the repo. One piece of Copilot-assisted code written without spending a premium request.

### Core -- Self-directed

Create a `copilot-instructions.md` that accurately describes your working style and this repo's conventions. Create at least one prompt file for a task you will actually do this afternoon in Lab 2. Then use inline completion (not chat) to write something small and compare the output to what you would have gotten without the instructions file.

### Extra Challenge -- Guided

1. Create a second prompt file: `/.github/prompts/test.prompt.md`
2. Describe how tests should be structured for your language (framework, file naming, assertion style)
3. Use Copilot Chat (one premium request) with the test prompt file explicitly attached as context
4. Compare the output to a chat request without the prompt file
5. In your `copilot-instructions.md`, add a `## Review checklist` section with three things Copilot should flag in any code it writes

### Extra Challenge -- Self-directed

Build out your Copilot configuration so it accurately represents the conventions for this repo. Add a test prompt file, a review checklist in the instructions file, and anything else you think would improve output quality for this afternoon's work. Measure the difference.

As you close Lab 1, preview the afternoon in 60 seconds: "This afternoon you implement a feature using these tools, run it through CI, and ship it to a live URL. Pick your language folder if you have not."

---

## 12:00 - 13:00 | Lunch

---

## 13:00 - 13:20 | Issues as specifications + agentic workflow demo (20 min)

**Mode:** explanation + instructor demo

### Issues as specifications (5 min)

A well-written issue is Copilot context. Show one bad issue ("improve performance") and one good issue (acceptance criteria, constraints, affected files, definition of done) side by side. Participants create two issues in their fork using the provided template -- one feature issue for Lab 2, one non-functional issue. Five minutes, not a full lab.

### Agentic workflow demo (15 min)

Instructor demo only. Requires Pro.

**Agent mode in VS Code:** assign a multi-file task from one of the issues. Show it planning, editing across files, running terminal commands, and self-correcting. Narrate the thought process and where it goes wrong. Show how you steer it back.

**Copilot coding agent (async):** assign an issue from the GitHub interface. Show the background session starting and the draft PR being created. Start it running now -- check back on the PR during Lab 2's Extra Challenge.

Say explicitly: "Agent mode in VS Code is available on all tiers but burns premium requests fast. The coding agent requires Pro. Free-tier attendees: watch, understand the pattern, make an informed call about whether to upgrade."

---

## 13:20 - 13:50 | Lab 2: Core sprint (30 min)

**Learning theme:** disciplined AI-assisted development using the tools you built this morning

**This lab runs as a timed sprint.** Hard stop at 13:50 regardless of progress. Anyone who hasn't finished copies the completed file from the `rescue/` folder for their language into their project folder, commits, and pushes (see [rescue/README.md](rescue/README.md)). They still own CI and deploy.

Participants do **Core only** in this slot. The guided/self-directed lab documents cover the task. Extra Challenge is available as optional homework.

### Checkpoint at 13:50 (hard stop)

Ask: "Who has a working Tool Comparison chart?" Anyone who does not: switch to the rescue branch now. Walk the room and verify. Do not let this bleed into the PR discussion.

---

## 13:50 - 14:15 | PRs, review, and AI-generated code (25 min)

**Mode:** demo + focused discussion

Easy: why PRs still matter when AI writes the code. What to look at first in a generated diff.

Hard: what AI gets wrong systematically (edge cases, security assumptions, idiomatic patterns). One heuristic: if you cannot explain every line, the PR is not ready. Show Copilot reviewing its own output and explain why that is useful but not sufficient -- it is the same model, with similar blind spots.

**Demo:** one bad PR, one good PR, 90 seconds each. Prompt Copilot for review comments on the bad PR.

**Discussion anchor:** "Do you trust AI-generated code in production?" Three answers from the room. This question always generates useful disagreement and is worth three minutes.

---

## 14:15 - 14:35 | Break

---

---

## 14:35 - 15:35 | Lab 3: CI with GitHub Actions (60 min)

**Learning theme:** automation as protection -- CI catches what humans miss, especially when AI is generating diffs at high volume

### Explain (10 min)

Easy: what CI does and why it matters more when AI is generating diffs at high volume.

Hard: events, jobs, steps, and caching in a multi-language workflow. CI as policy enforcement. Then the Azure explanation:

**Azure authentication (demo only, 5 min within this block):**

"If you were deploying to Azure instead of Pages, here is what the authentication looks like. The official Azure documentation for this currently has errors in the OIDC setup steps, so I am showing you the correct pattern."

Show OIDC federation: the `azure/login` action with `client-id`, `tenant-id`, and `subscription-id`. No stored secrets in the workflow file -- GitHub Actions gets a short-lived token automatically. Compare to the older service principal with secret approach and explain why OIDC is the correct default. Show where the current docs go wrong (subject claim format). This takes five minutes and gives everyone a correct mental model of production deployment auth, even though they are deploying to Pages today.

### Lab (50 min)

Participants work through the lab documents in `labs/lab-3-ci-deploy/`. Core task: add workflow structure (name, triggers, job definition) around the pre-written CI steps, create a branch, open a PR, and get CI green.

---

## 15:35 - 15:45 | Vibecoding and GitHub Spark (10 min)

**Mode:** instructor demo only. **Drop entirely if Lab 3 runs long.**

Frame it: "You have spent the day writing disciplined, context-grounded, reviewed code. Now the other end of the spectrum."

Define vibecoding (Andrej Karpathy, early 2025): natural language description, AI output accepted with minimal review. Fast, creative, brittle.

**Demo:**
- Build a small throwaway app in Spark from a natural language prompt
- Show what it produces in 2-3 minutes
- Show where it breaks or needs correction

No discussion slot -- if participants want to talk about it, that happens during Lab 4 or wrap-up.

**Backup:** have a recorded screen capture ready.

---

## 15:45 - 16:30 | Lab 4: Deploy to GitHub Pages + wrap-up (45 min)

**Learning theme:** deployment as the result -- the live URL is the payoff for the whole day

### Lab (30 min)

Participants work through the lab documents in `labs/lab-4-deploy/`. Core task: configure GitHub Pages, add Marketplace actions to the deploy workflow, merge the PR from Lab 3, and verify the live URL.

### Wrap-up (15 min)

Three questions, two to three answers each:

1. "Where did AI tools actually speed you up today?"
2. "Where did they slow you down or produce something you had to fix?"
3. "What would you change about your own workflow based on today?"

Close: point to the repo for all materials, instructions files, prompt file templates, and the `github/awesome-copilot` collection. Thank the room.

---

## Revised timing summary

| Block | Start | End | Duration | Change from original |
|---|---|---|---|---|
| Welcome, check, poll | 09:00 | 09:20 | 20 min | none |
| Lab 0: Codespaces + debrief | 09:20 | 10:15 | 55 min | none |
| Break | 10:15 | 10:35 | 20 min | +5 min |
| Copilot tiers + limits | 10:35 | 11:05 | 30 min | shifts 5 min |
| Lab 1: Instructions, prompts, skills | 11:05 | 12:00 | 55 min | -5 min, morning wrap removed |
| Lunch | 12:00 | 13:00 | 60 min | starts 10 min earlier |
| Issues as specs + agentic demo | 13:00 | 13:20 | 20 min | shifts 10 min |
| **Lab 2: Core sprint only** | **13:20** | **13:50** | **30 min** | **-40 min, hard stop** |
| PRs + review + discussion | 13:50 | 14:15 | 25 min | +5 min |
| Break | 14:15 | 14:35 | 20 min | moved up 45 min |
| Lab 3: CI + Actions | 14:35 | 15:35 | 60 min | CI only, deploy is Lab 4 |
| Vibecoding + Spark (trim/drop if late) | 15:35 | 15:45 | 10 min | -10 min, demo only |
| Lab 4: Deploy + wrap-up | 15:45 | 16:30 | 45 min | +10 min, earlier finish |
| **Net teaching time** | | | **5h 50min** | |
| **Breaks + lunch** | | | **1h 40min** | |
| **Total** | | | **7h 30min** | |

---

## Three things to note about this structure

**The two-mode format adds prep work.** Every lab now needs two written versions. For each lab that is four documents (Core guided, Core self-directed, Extra Challenge guided, Extra Challenge self-directed). Budget time to write these before the workshop, and put them in the repo so participants can access them from their Codespace without switching windows.

**Lab 2 is the tightest lab.** 30 minutes for a core sprint is realistic only if participants focus on the single `by_tool` task and do not attempt the Extra Challenge in-room. The `rescue/` folder is the safety net -- verify the rescue files work before the day. The hard stop at 13:50 is non-negotiable; Lab 3 depends on everyone having working code.

**Vibecoding is the flex slot.** If Lab 3 runs long (CI labs often do), Vibecoding is the first thing to drop. 10 minutes is a fine demo; if you are behind at 15:35, skip it entirely. The live deployed URL at the end of Lab 4 is what participants remember.