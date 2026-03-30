## The app: AI Experiment Log with Visual Summary

Each attendee maintains their own log of AI experiments run during the workshop. Every log entry captures:

- Tool used (Copilot inline, Copilot Chat, agent mode, Spark, etc.)
- Task attempted
- What you expected
- What actually happened
- Verdict: Faster / Same / Slower / Surprising

The Pages deployment renders this as a personal dashboard with:

- A verdict breakdown chart (how many experiments fell into each category)
- A tool comparison view (which tools produced the most "Faster" verdicts)
- A scrollable timeline of all entries with expandable detail

The data lives in a JSON file committed to the repo. The Actions workflow rebuilds and redeploys Pages every time a new entry is committed. No external backend required.

---

## Why this holds together

**The data is real by end of day.** Attendees have been using Copilot since 09:00. By the time they deploy at 16:00 they have actual opinions about what helped and what did not. The log is not a demo dataset -- it is evidence from their own experience.

**The visual is earned, not decorative.** A chart showing "agent mode: 2 faster, 1 slower, 1 surprising" means something because the attendee generated that data themselves. This is different from a chart over synthetic data.

**The Pages deployment is genuinely dynamic.** Every commit triggers a rebuild. The attendee can add an entry after the workshop and see the chart update. This makes the CI/Deploy lab feel like it produces something alive, not a static artifact.

**It connects directly to the workshop's closing debrief.** The three wrap-up questions ("where did AI speed you up, slow you down, or surprise you?") are the same questions the app answers visually. The debrief becomes a conversation about what attendees see in their own dashboard.

---

## What the API looks like across the three language folders

Each language implements the same four endpoints:

- `GET /entries` -- return all log entries from the JSON data file
- `POST /entries` -- add a new entry, write to the JSON data file
- `GET /summary` -- return aggregated counts by tool and verdict
- `GET /entries/:id` -- return one entry with full detail

The JSON data file (`/data/experiments.json`) is the source of truth. It is committed to the repo. The API reads and writes it. Actions commits any changes made through the API and triggers a rebuild.

The **feature added in Lab 2** is the `GET /summary` endpoint with aggregation logic. This is a good Lab 2 target because:
- It requires real logic (grouping, counting, sorting), not just a data passthrough
- The output is immediately visible in the frontend chart
- Copilot inline completion handles the aggregation well when given a clear comment describing the expected shape of the output
- It is scoped well -- one endpoint, testable, verifiable in the browser

---

## What the frontend looks like

The SPA has three views:

**Log view** (available from the start, pre-populated with two example entries so the page is not empty): a chronological list of entries with tool, task, and verdict visible. Clicking an entry expands the full detail.

**Dashboard view** (unlocked when `GET /summary` is implemented in Lab 2): two charts side by side.
- Left: verdict breakdown as a horizontal bar chart or donut (Faster / Same / Slower / Surprising)
- Right: tool comparison as a grouped bar chart showing verdict distribution per tool

**Add entry form**: a simple form committing a new entry. In the lab context this writes directly to the JSON file via the running API. After the workshop, attendees can add entries by editing the JSON file directly and pushing, which triggers a rebuild.

The charts should be built with a library that works in a Pages SPA without a build step -- Chart.js or a similar CDN-based option is the right call here. Do not make attendees configure a charting build pipeline.

---

## What the repo structure looks like

```
/
├── .devcontainer/
├── .github/
│   ├── copilot-instructions.md
│   ├── prompts/
│   │   ├── feature.prompt.md
│   │   └── test.prompt.md
│   ├── skills/
│   │   └── experiment-logger/
│   └── workflows/
│       ├── ci.yml
│       └── deploy.yml
├── data/
│   └── experiments.json
├── frontend/
│   ├── index.html
│   └── app.js
├── node/
│   └── server.js
├── python/
│   └── app.py
└── dotnet/
    └── Api.cs
```

The frontend is shared. The data file is shared. Each language folder implements the same API contract against the same data file.

---

## The Lab 2 feature in concrete terms

The `GET /summary` endpoint should return this shape:

```json
{
  "by_verdict": {
    "Faster": 4,
    "Same": 2,
    "Slower": 1,
    "Surprising": 3
  },
  "by_tool": {
    "Copilot inline": { "Faster": 3, "Same": 1, "Slower": 0, "Surprising": 1 },
    "Copilot Chat": { "Faster": 1, "Same": 1, "Slower": 1, "Surprising": 1 },
    "Agent mode": { "Faster": 0, "Same": 0, "Slower": 1, "Surprising": 1 },
    "Spark": { "Faster": 0, "Same": 0, "Slower": 0, "Surprising": 1 }
  },
  "total": 10
}
```

A comment in the starter file describing this shape is enough for Copilot inline completion to generate the correct aggregation logic in any of the three languages. The guided lab instruction tells participants to write the comment first, then let Copilot complete the function body, then verify the output against the expected shape. This is a clean demonstration of comment-driven inline completion that does not require chat.

---

## One risk to flag

The JSON-file-as-database pattern is honest and simple, but it breaks under concurrent writes. Two attendees cannot share a backend -- and they should not, because the whole point is that each person's log is personal and lives in their own fork. Make this explicit in the lab instructions: "This is your personal log. It lives in your fork. No one else sees it unless you share the URL." This prevents anyone from expecting a shared real-time experience and frames the architecture choice correctly.

The Extra Challenge for Lab 2 could be adding a simple concurrency guard (read-modify-write with a file lock, or an optimistic check on the entry count before writing). This is a realistic engineering problem and a good Copilot test -- it is the kind of edge case where AI-generated code often needs correction.
---

## The core principle

Attendees should spend their time on the learning goal of each lab, not on boilerplate that is incidental to it. That means the starter repo ships with everything except what each lab is specifically teaching.

The risk of providing too little: participants spend Lab 0 debugging a broken frontend instead of learning Codespaces. The risk of providing too much: participants feel like they are just enabling things rather than building anything. The line sits at "working app with a deliberate gap that the lab fills."

---

## What is provided in the starter repo

**Frontend (complete, do not touch):** the full SPA including the log view, the add-entry form, and the dashboard view with charts. The dashboard view renders a "summary not yet available" state gracefully when `GET /summary` does not exist. Once that endpoint is implemented in Lab 2, the dashboard unlocks automatically. Attendees do not write frontend code -- they are here for the backend and the pipeline, not CSS.

**Data file:** `/data/experiments.json` with two pre-populated example entries. Not empty, so the log view shows something on first run. Not full, so there is room to add real entries during the day.

**API skeleton in all three language folders:** the three simpler endpoints are provided and working:
- `GET /entries` -- reads and returns the JSON file
- `POST /entries` -- appends a new entry and writes the file
- `GET /entries/:id` -- returns one entry by ID

What is deliberately missing from the starter: `GET /summary`. The function stub is there with a comment describing the expected output shape. The endpoint is registered in the router. It returns a 501 Not Implemented. This is what Lab 2 fills in.

**GitHub Actions workflows:** provided but not enabled. The CI workflow file exists in `.github/workflows/ci.yml` but is written to a non-standard path so it does not trigger automatically. Lab 3 moves it into place and enables it. This gives attendees the full workflow content to read and understand rather than writing it from scratch, but requires a deliberate action to activate.

**Copilot configuration folder:** the `.github/` folder exists with the correct subfolder structure (`prompts/`, `skills/`, `agents/`). The files inside it are empty or contain placeholder comments. Lab 1 fills these in.

**Lab instructions:** in the repo at `/labs/`, structured as described below.

---

## What attendees create in each lab

### Lab 0: nothing new is created

The learning goal is the environment, not the code. Attendees fork, open a Codespace, run the app, and understand what they are looking at. The Extra Challenge modifies the devcontainer, which is creation, but it is not part of the app itself.

At the end of Lab 0: a running app in a Codespace, two example log entries visible in the browser, the dashboard showing "summary not yet available."

### Lab 1: Copilot configuration files

Attendees create:
- `/.github/copilot-instructions.md` (from scratch, guided by the lab)
- `/.github/prompts/feature.prompt.md` (from scratch)
- `/.github/prompts/test.prompt.md` (Extra Challenge)

These files do not change the running app. They change how Copilot behaves in Lab 2. The connection between Lab 1 and Lab 2 should be made explicit: "What you write now is what Copilot will use when you implement the feature this afternoon."

At the end of Lab 1: the repo has working Copilot configuration committed. The app is unchanged. The dashboard still shows "summary not yet available."

### Lab 2: the summary endpoint

Attendees implement `GET /summary` in their chosen language folder. This is the only lab where they write application code. When it works, the dashboard unlocks and the charts appear.

This is the moment the app becomes theirs. They have real entries (added during the day, or the two starters), and the chart reflects actual data. If they only have two entries, the chart is sparse but correct. Frame this: "Add a few more entries before you implement the endpoint so the chart is interesting when it renders."

At the end of Lab 2: working `GET /summary`, charts visible in the browser, feature committed and referenced to the issue.

### Lab 3: CI and deploy workflows

Attendees activate and configure the provided workflows, adjust the language matrix, make the CI check pass, and deploy to Pages. What they create is the enabled, working pipeline -- not the workflow file content, but the configured, green version of it.

The deploy step is the payoff: a public URL with their name in it, showing their own data, rendered as a dashboard.

At the end of Lab 3: a live Pages URL. The dashboard shows real experiment data. The CI check is green on main.

---

## How the app grows across the day

| Time | State of the app |
|---|---|
| 09:20 (Lab 0 start) | Not yet running |
| 10:15 (Lab 0 end) | Running in Codespace, two example entries, no summary |
| 12:00 (Lab 1 end) | Same app, but Copilot configuration committed |
| 14:40 (Lab 2 end) | Summary endpoint working, dashboard live locally |
| 16:50 (Lab 3 end) | Deployed to Pages, publicly accessible, CI green |

The progression is visible and cumulative. Each lab adds one layer. Nothing gets thrown away.

---

## Lab instruction repo structure

```
/labs/
├── README.md                        ← which lab to start with, how to pick your mode
├── lab-0-codespaces/
│   ├── core-guided.md
│   ├── core-self-directed.md
│   ├── challenge-guided.md
│   └── challenge-self-directed.md
├── lab-1-copilot-config/
│   ├── core-guided.md
│   ├── core-self-directed.md
│   ├── challenge-guided.md
│   └── challenge-self-directed.md
├── lab-2-summary-endpoint/
│   ├── core-guided.md
│   ├── core-self-directed.md
│   ├── challenge-guided.md
│   └── challenge-self-directed.md
└── lab-3-ci-deploy/
    ├── core-guided.md
    ├── core-self-directed.md
    ├── challenge-guided.md
    └── challenge-self-directed.md
```

The `/labs/README.md` should explain the two-tier, two-mode system and tell people how to navigate it. Keep it to one screen -- if someone has to scroll to find which file to open, you will lose people at the start of every lab.

Each lab file should open with three things before any instructions:

1. **Goal:** one sentence, what exists at the end of this lab that did not exist before
2. **Time:** how long Core is expected to take, and the signal to move to Challenge
3. **You will need:** any prerequisite state (e.g., "your Codespace should be running from Lab 0")

---

## Practical note on the "not yet enabled" workflow approach

Making the CI workflow file exist but not trigger is easy in practice: name the file `ci.yml.disabled` or put it in a `/workflow-templates/` folder outside `.github/workflows/`. Lab 3 instructs attendees to move or rename it. This is more instructive than just uncommenting a line because it forces them to understand that the `.github/workflows/` path is what GitHub looks for, not just that the file exists.

An alternative is to have the workflow file in the correct location from day one but with a `paths` filter that only triggers on a specific branch that does not exist yet. Lab 3 creates that branch. Either approach works -- the rename approach is simpler to explain in guided instructions.